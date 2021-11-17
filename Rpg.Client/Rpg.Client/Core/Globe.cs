using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Globe
    {
        public Globe()
        {
            // First variant of the names.
            /*
             * "Поле брани", "Дикое болото", "Черные топи", "Лес колдуна", "Нечистивая\nяма",
             * "Мыс страха", "Тропа\nпогибели", "Кладбише\nпроклятых", "Выжженая\nдеревня", "Холм тлена"
             */

            var biomes = GenerateBiomes();

            Biomes = biomes;
            CurrentBiome = biomes.Single(x => x.IsStart);
        }

        public Combat? ActiveCombat { get; set; }

        public IEnumerable<Biome> Biomes { get; }

        public Biome? CurrentBiome { get; set; }

        public Event? CurrentEvent { get; internal set; }

        public EventNode? CurrentEventNode { get; set; }

        public bool IsNodeInitialied { get; set; }

        public Player? Player { get; set; }

        public void UpdateNodes(IDice dice)
        {
            // Reset all combat states.
            var biomes = Biomes.Where(x => x.IsAvailable).ToArray();
            foreach (var biom in biomes)
            {
                foreach (var node in biom.Nodes)
                {
                    node.CombatSequence = null;
                    node.AssignedEvent = null;
                }

                if (biom.IsComplete && biom.UnlockBiome is not null)
                {
                    var unlockedBiom = Biomes.Single(x => x.Type == biom.UnlockBiome);

                    unlockedBiom.IsAvailable = true;
                }
            }

            // Create new combats
            foreach (var biome in biomes)
            {
                if (biome.Level < 10)
                {
                    var availableNodes = biome.Nodes.Where(x => x.IsAvailable).ToArray();
                    Debug.Assert(availableNodes.Any(), "At least of one node expected to be available.");
                    var rollCount = Math.Min(availableNodes.Length, 3);
                    var nodesWithCombats = dice.RollFromList(availableNodes, rollCount).ToArray();

                    var combatCounts = GetCombatCounts(biome.Level);
                    var combatLevelAdditionalList = new[] { 0, -1, 3 };
                    var selectedNodeCombatCount = dice.RollFromList(combatCounts, 3).ToArray();
                    var combatLevelAdditional = 0;

                    var combatToTrainingIndex = dice.RollArrayIndex(nodesWithCombats);

                    for (var i = 0; i < nodesWithCombats.Length; i++)
                    {
                        var selectedNode = nodesWithCombats[i];
                        var targetCombatCount = selectedNodeCombatCount[i];

                        var combatLevel = biome.Level + combatLevelAdditionalList[combatLevelAdditional];
                        var combatList = new List<CombatSource>();
                        for (var combatIndex = 0; combatIndex < targetCombatCount; combatIndex++)
                        {
                            var units = CreateReqularMonsters(selectedNode, dice, biome, combatLevel);

                            var combat = new CombatSource
                            {
                                Level = combatLevel,
                                EnemyGroup = new Group
                                {
                                    Units = units
                                },
                                IsTrainingOnly = combatToTrainingIndex == i
                            };

                            combatList.Add(combat);
                        }

                        var combatSequence = new CombatSequence
                        {
                            Combats = combatList
                        };

                        selectedNode.CombatSequence = combatSequence;

                        combatLevelAdditional++;
                    }
                }
                else
                {
                    var combatLevelAdditional = 0;

                    var availableNodes = biome.Nodes.Where(x => x.IsAvailable).ToArray();
                    Debug.Assert(availableNodes.Any(), "At least of one node expected to be available.");

                    var rollCount = Math.Min(availableNodes.Length, 3);
                    var nodesWithCombats = dice.RollFromList(availableNodes, 3).ToArray();

                    var combatCounts = GetCombatCounts(biome.Level);
                    var combatLevelAdditionalList = new[] { 0, -1, 3 };
                    var selectedNodeCombatCount = dice.RollFromList(combatCounts, 2).ToArray();

                    var combatToTrainingIndex = dice.RollArrayIndex(nodesWithCombats);

                    for (var i = 0; i < nodesWithCombats.Length; i++)
                    {
                        var selectedNode = nodesWithCombats[i];

                        // boss level
                        if (i == 0)
                        {
                            var bossesOfCurrentBiom = UnitSchemeCatalog.AllUnits
                                .Where(x => x.BossLevel is not null && x.Biome == biome.Type).ToList();
                            var bossUnitScheme = dice.RollFromList(bossesOfCurrentBiom, 1).Single();

                            var combatList = new[]
                            {
                                new CombatSource
                                {
                                    IsBossLevel = true,
                                    EnemyGroup = new Group
                                    {
                                        Units = new[]
                                        {
                                            new Unit(
                                                bossUnitScheme,
                                                biome.Level)
                                        }
                                    },
                                    IsTrainingOnly = combatToTrainingIndex == i
                                }
                            };

                            var combatSequence = new CombatSequence
                            {
                                Combats = combatList
                            };

                            selectedNode.CombatSequence = combatSequence;
                        }
                        else
                        {
                            var combatLevel = biome.Level + combatLevelAdditionalList[combatLevelAdditional];
                            var targetCombatCount = selectedNodeCombatCount[i - 1];

                            var combatList = new List<CombatSource>();

                            for (var combatIndex = 0; combatIndex < targetCombatCount; combatIndex++)
                            {
                                var units = CreateReqularMonsters(selectedNode, dice, biome, combatLevel);

                                var combat = new CombatSource
                                {
                                    Level = combatLevel,
                                    EnemyGroup = new Group
                                    {
                                        Units = units
                                    },
                                    IsTrainingOnly = combatToTrainingIndex == i
                                };

                                combatList.Add(combat);
                            }

                            var combatSequence = new CombatSequence
                            {
                                Combats = combatList
                            };

                            selectedNode.CombatSequence = combatSequence;
                        }

                        combatLevelAdditional++;
                    }
                }
            }

            // create dialogs of nodes with combat
            foreach (var biome in biomes)
            {
                var nodesWithCombat = biome.Nodes.Where(x => x.CombatSequence is not null).ToArray();

                AssignEventToNodesWithCombat(biome, dice, nodesWithCombat);
            }

            Updated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Goal:
        /// Do not pick used event when there are unused event in the catalog.
        /// Algorithm:
        /// 1. We know the counters of all events in the catalog. So calc min and max counter to split all the event into ranked
        /// groups.
        /// 2. Next start with a group with min rank. And select random event from this group.
        /// 3. We take next group when we can't take any event from current group.
        /// </summary>
        /// <param name="dice"></param>
        /// <param name="nodesWithCombat"></param>
        private static void AssignEventToNodesWithCombat(Biome biome, IDice dice, GlobeNode[] nodesWithCombat)
        {
            var availableEvents = EventCatalog.Events
                .Where(x => (x.IsUnique && x.Counter == 0) || (!x.IsUnique))
                .Where(x => (x.Biome is not null && x.Biome == biome.Type) || (x.Biome is null))
                .Where(x => (x.RequiredBiomeLevel is not null && x.RequiredBiomeLevel <= biome.Level) ||
                            (x.RequiredBiomeLevel is null))
                .Where(x => IsUnlocked(x, EventCatalog.Events));
            var availableEventList = availableEvents.ToList();

            foreach (var node in nodesWithCombat)
            {
                if (!availableEventList.Any())
                {
                    // There are no available events.
                    // But there are nodes to assign events.
                    // Just break attempts. We can do nothing.
                    break;
                }

                var nodeEvents = availableEventList.Where(x =>
                    (x.ApplicableOnlyFor is null) ||
                    (x.ApplicableOnlyFor is not null && x.ApplicableOnlyFor.Contains(node.Sid))).ToArray();
                if (nodeEvents.Any())
                {
                    var highPriorityEvent = nodeEvents.FirstOrDefault(x => x.IsHighPriority);
                    if (highPriorityEvent is not null)
                    {
                        node.AssignedEvent = highPriorityEvent;
                    }
                    else
                    {
                        var roll = dice.Roll(1, 10);
                        if (roll > 5)
                        {
                            var minCounter = nodeEvents.Min(x => x.Counter);
                            var currentRankEventList = nodeEvents.Where(x => x.Counter == minCounter).ToArray();
                            var rolledEvent = dice.RollFromList(currentRankEventList, 1).Single();

                            node.AssignedEvent = rolledEvent;
                        }
                    }

                    availableEventList.Remove(node.AssignedEvent);
                }
            }
        }

        private static IEnumerable<Unit> CreateReqularMonsters(GlobeNode node, IDice dice, Biome biom, int combatLevel)
        {
            var availableMonsters = UnitSchemeCatalog.AllUnits
                .Where(x => x.BossLevel is null && x.Biome == biom.Type && x.NodeIndexes.Contains(node.Index)).ToList();
            var rolledUnits = new List<UnitScheme>();
            var monsterCount = dice.Roll(1, Math.Min(3, availableMonsters.Count));
            for (var i = 0; i < monsterCount; i++)
            {
                var scheme = dice.RollFromList(availableMonsters, 1).Single();

                rolledUnits.Add(scheme);

                if (scheme.IsUnique)
                {
                    // Remove all unique monsters from roll list.
                    availableMonsters.RemoveAll(x => x.IsUnique);
                }
            }

            var units = new List<Unit>();
            foreach (var unitScheme in rolledUnits)
            {
                var unitLevel = GetUnitLevel(combatLevel);
                var unit = new Unit(unitScheme, unitLevel);
                units.Add(unit);
            }

            return units;
        }

        private static Biome[] GenerateBiomes()
        {
            const int BIOME_MIN_LEVEL_STEP = 12;
            const int BIOME_NODE_COUNT = 8;

            return new[]
            {
                new Biome(0, BiomeType.Slavic)
                {
                    IsAvailable = true,
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Slavic),
                            Sid = GetNodeSid(x, BiomeType.Slavic),
                            IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Chinese,
                    IsStart = true
                },
                new Biome(BIOME_MIN_LEVEL_STEP, BiomeType.Chinese)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x,
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Chinese),
                            Sid = GetNodeSid(x, BiomeType.Chinese),
                            IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Egyptian
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 2, BiomeType.Egyptian)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x, EquipmentItem = GetEquipmentItem(x, BiomeType.Egyptian),
                            Sid = GetNodeSid(x, BiomeType.Egyptian), IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Greek
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 3, BiomeType.Greek)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            Index = x, EquipmentItem = GetEquipmentItem(x, BiomeType.Greek),
                            Sid = GetNodeSid(x, BiomeType.Greek), IsAvailable = GetStartAvailability(x)
                        }
                    ).ToArray(),
                    IsFinal = true
                }
            };
        }

        private static int[] GetCombatCounts(int level)
        {
            return level switch
            {
                0 or 1 => new[] { 1, 1, 1 },
                2 => new[] { 1, 1, 1, 3 },
                > 3 and <= 4 => new[] { 1, 1, 1, 3, 3 },
                > 5 and <= 7 => new[] { 1, 3, 3, 3, 5 },
                > 8 and <= 10 => new[] { 3, 3, 3, 5, 5 },
                > 10 => new[] { 3, 5, 5 },
                _ => new[] { 1, 1, 1, 1, 1, 1, 3, 3, 3, 5, 5 }
            };
        }

        private static EquipmentItemType? GetEquipmentItem(int nodeIndex, BiomeType biomType)
        {
            switch (biomType)
            {
                case BiomeType.Slavic:
                    {
                        return nodeIndex switch
                        {
                            // TODO Rewrite to pattern matching with range
                            0 => EquipmentItemType.Warrior,
                            1 => EquipmentItemType.Archer,
                            2 => EquipmentItemType.Herbalist,
                            3 => EquipmentItemType.Warrior,
                            4 => EquipmentItemType.Archer,
                            5 => EquipmentItemType.Herbalist,

                            _ => null
                        };
                    }

                case BiomeType.Egyptian:
                    {
                        return nodeIndex switch
                        {
                            0 => EquipmentItemType.Priest,
                            3 => EquipmentItemType.Priest,
                            _ => null
                        };
                    }

                default:
                    return null;
            }
        }

        private static GlobeNodeSid GetNodeSid(int nodeIndex, BiomeType biomType)
        {
            var sidIndex = (int)biomType + nodeIndex + 1;
            var sid = (GlobeNodeSid)sidIndex;
            return sid;
        }

        private static bool GetStartAvailability(int nodeIndex)
        {
            return nodeIndex == 0;
        }

        private static int GetUnitLevel(int combatLevel)
        {
            // +1 because combat starts with zero.
            // But a unit's level have to starts with 1.
            return combatLevel + 1;
        }

        private static bool IsUnlocked(Event testedEvent, IEnumerable<Event> events)
        {
            if (testedEvent.RequiredEventsCompleted is null)
            {
                return true;
            }

            var completedEvents = events.Where(x => x.Completed).ToArray();
            foreach (var eventSid in testedEvent.RequiredEventsCompleted)
            {
                if (eventSid is null)
                {
                    continue;
                }

                var foundCompletedEvent = completedEvents.Any(x => x.Title == eventSid);
                if (!foundCompletedEvent)
                {
                    return false;
                }
            }

            return true;
        }

        public event EventHandler? Updated;
    }
}