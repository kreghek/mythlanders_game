using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class BiomeGenerator : IBiomeGenerator
    {
        private const int BIOME_NODE_COUNT = 8;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public BiomeGenerator(IDice dice, IUnitSchemeCatalog unitSchemeCatalog, IEventCatalog eventCatalog)
        {
            _dice = dice;
            _unitSchemeCatalog = unitSchemeCatalog;
            _eventCatalog = eventCatalog;
        }

        private static int[] GetCombatSequenceLength(int level)
        {
            return level switch
            {
                0 => new[] { 1, 1, 1 },
                1 => new[] { 1, 1, 3 },
                2 => new[] { 1, 1, 3, 3 },
                > 3 and <= 4 => new[] { 1, 3, 3, 5 },
                > 5 and <= 7 => new[] { 3, 3, 3, 5 },
                > 8 and <= 10 => new[] { 3, 3, 3, 5, 5 },
                > 10 => new[] { 3, 5, 5 },
                _ => new[] { 1, 1, 1, 1, 1, 1, 3, 3, 3, 5, 5 }
            };
        }

        private static EquipmentItemType? GetEquipmentItem(int nodeIndex, BiomeType biomeType)
        {
            switch (biomeType)
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

        private static GlobeNodeSid GetNodeSid(int nodeIndex, BiomeType biomeType)
        {
            var sidIndex = (int)biomeType + nodeIndex + 1;
            var sid = (GlobeNodeSid)sidIndex;
            return sid;
        }

        private static bool GetStartAvailability(int nodeIndex)
        {
            return nodeIndex == 0;
        }

        private static bool IsBossLevel(GlobeLevel globeLevel, int completeBiomesCount)
        {
            return globeLevel.Level >= 10 + completeBiomesCount * 12;
        }

        private static (GlobeNode, bool)[] RollNodesWithCombats(IDice dice,
            IList<GlobeNode> availableNodes, GlobeLevel globeLevel, IEnumerable<Biome> biomes)
        {
            const int COMBAT_UNDER_ATTACK_COUNT = 3;
            var BOSS_LOCATION_SIDS = biomes.SelectMany(x => x.Nodes).Where(x => x.IsLast).Select(x => x.Sid).ToArray();
            var completeBiomesCount = biomes.Count();

            var nodeList = new List<(GlobeNode, bool)>(3);
            GlobeNode? bossLocation = null; //availableNodes.SingleOrDefault(x => BOSS_LOCATION_SIDS.Contains(x.Sid));
            int targetCount;
            if (IsBossLevel(globeLevel, completeBiomesCount) && bossLocation is not null /*&& !biome.IsComplete*/)
            {
                nodeList.Add(new(bossLocation, true));
                targetCount = Math.Min(availableNodes.Count, COMBAT_UNDER_ATTACK_COUNT - 1);
            }
            else
            {
                targetCount = Math.Min(availableNodes.Count, COMBAT_UNDER_ATTACK_COUNT);
            }

            var regularLocations = dice.RollFromList(availableNodes, targetCount);
            foreach (var location in regularLocations)
            {
                nodeList.Add(new(location, false));
            }

            return nodeList.ToArray();
        }

        public IReadOnlyList<Biome> GenerateStartState()
        {
            return new[]
            {
                new Biome(BiomeType.Slavic)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Slavic),
                            Sid = GetNodeSid(x, BiomeType.Slavic),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            BiomeType = BiomeType.Slavic
                        }
                    ).ToArray()
                },
                new Biome(BiomeType.Chinese)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Chinese),
                            Sid = GetNodeSid(x, BiomeType.Chinese),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            BiomeType = BiomeType.Chinese
                        }
                    ).ToArray()
                },
                new Biome(BiomeType.Egyptian)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Egyptian),
                            Sid = GetNodeSid(x, BiomeType.Egyptian),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            BiomeType = BiomeType.Egyptian
                        }
                    ).ToArray()
                },
                new Biome(BiomeType.Greek)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Greek),
                            Sid = GetNodeSid(x, BiomeType.Greek),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            BiomeType = BiomeType.Greek
                        }
                    ).ToArray()
                },
                new Biome(BiomeType.Cosmos)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Cosmos),
                            Sid = GetNodeSid(x, BiomeType.Cosmos),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            BiomeType = BiomeType.Cosmos
                        }
                    ).ToArray()
                }
            };
        }

        public void CreateCombatsInBiomeNodes(IEnumerable<Biome> biomes, GlobeLevel globeLevel)
        {
            var availableNodes = biomes.SelectMany(x => x.Nodes).Where(x => x.IsAvailable).ToArray();
            Debug.Assert(availableNodes.Any(), "At least of one node expected to be available.");
            var nodesWithCombats = RollNodesWithCombats(_dice, availableNodes, globeLevel, biomes);

            var combatCounts = GetCombatSequenceLength(globeLevel.Level);
            var combatLevelAdditionalList = new[]
            {
                0, -1, 3
            };
            var selectedNodeCombatCount = _dice.RollFromList(combatCounts, 3).ToArray();
            var combatLevelAdditional = 0;

            var globeContext = new MonsterGenerationGlobeContext(globeLevel, biomes);

            for (var locationIndex = 0; locationIndex < nodesWithCombats.Length; locationIndex++)
            {
                var selectedNode = nodesWithCombats[locationIndex];
                var targetCombatSequenceLength = selectedNode.Item2 ? 1 : selectedNodeCombatCount[locationIndex];

                var combatLevel = globeLevel.MonsterLevel + combatLevelAdditionalList[combatLevelAdditional];
                var combatList = new List<CombatSource>();

                for (var combatIndex = 0; combatIndex < targetCombatSequenceLength; combatIndex++)
                {
                    var units = MonsterGeneratorHelper
                        .CreateMonsters(selectedNode.Item1, _dice, combatLevel, _unitSchemeCatalog, globeContext)
                        .ToArray();

                    var combat = new CombatSource
                    {
                        Level = combatLevel,
                        EnemyGroup = new Group(),
                        IsTrainingOnly = /*combatToTrainingIndex == locationIndex &&
                                         biome.Nodes.Where(x => x.IsAvailable).Count() == 4*/ false,
                        IsBossLevel = selectedNode.Item2
                    };

                    for (var slotIndex = 0; slotIndex < units.Length; slotIndex++)
                    {
                        var unit = units[slotIndex];
                        combat.EnemyGroup.Slots[slotIndex].Unit = unit;
                    }

                    combatList.Add(combat);
                }

                var combatSequence = new CombatSequence
                {
                    Combats = combatList
                };

                selectedNode.Item1.CombatSequence = combatSequence;

                combatLevelAdditional++;
            }
        }

        public void CreateStartCombat(Biome startBiome)
        {
            var combat = new CombatSource
            {
                Level = 1,
                EnemyGroup = new Group()
            };

            var combatSequence = new CombatSequence
            {
                Combats = new[] { combat }
            };

            var startNode = startBiome.Nodes.Single(x => x.Sid == GlobeNodeSid.Thicket);
            startNode.CombatSequence = combatSequence;

            var startEvent = _eventCatalog.Events.Single(x => x.IsGameStart);

            startNode.AssignedEvent = startEvent;

            combat.EnemyGroup.Slots[0].Unit =
                new Unit(_unitSchemeCatalog.AllMonsters.Single(x => x.Name == UnitName.Marauder), 2);
            combat.EnemyGroup.Slots[1].Unit =
                new Unit(_unitSchemeCatalog.AllMonsters.Single(x => x.Name == UnitName.BlackTrooper), 1);
            combat.EnemyGroup.Slots[2].Unit =
                new Unit(_unitSchemeCatalog.AllMonsters.Single(x => x.Name == UnitName.BlackTrooper), 1);
        }
    }
}