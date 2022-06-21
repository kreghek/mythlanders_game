using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DemoBiomeGenerator : IBiomeGenerator
    {
        private readonly IDice _dice;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public DemoBiomeGenerator(IDice dice, IUnitSchemeCatalog unitSchemeCatalog)
        {
            _dice = dice;
            _unitSchemeCatalog = unitSchemeCatalog;
        }

        private static int[] GetCombatSequenceLength(int level)
        {
            return level switch
            {
                0 => new[] { 1, 1, 1 },
                1 => new[] { 3, 3, 3 },
                2 => new[] { 1, 3, 3, 3 },
                > 3 and <= 4 => new[] { 1, 3, 3, 5 },
                > 5 and <= 7 => new[] { 3, 3, 3, 5 },
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

        private static GlobeNodeSid GetNodeSid(int nodeIndex, BiomeType biomeType)
        {
            var demoLocationsDict = new Dictionary<BiomeType, GlobeNodeSid[]>
            {
                {
                    BiomeType.Slavic,
                    new[]
                    {
                        GlobeNodeSid.Thicket, GlobeNodeSid.Battleground, GlobeNodeSid.DestroyedVillage,
                        GlobeNodeSid.Swamp
                    }
                }
            };

            return demoLocationsDict[biomeType][nodeIndex];
        }

        private static bool GetStartAvailability(int nodeIndex)
        {
            return nodeIndex == 0;
        }

        private static bool IsBossAvailable(Biome biome, GlobeLevel globeLevel)
        {
            return globeLevel.Level >= 5;
        }

        private static (GlobeNode, bool)[] RollNodesWithCombats(Biome biome, IDice dice,
            IList<GlobeNode> availableNodes, GlobeLevel globeLevel)
        {
            const int COMBAT_UNDER_ATTACK_COUNT = 3;
            const GlobeNodeSid BOSS_LOCATION_SID = GlobeNodeSid.Swamp;

            var nodeList = new List<(GlobeNode, bool)>(3);
            var bossLocation = availableNodes.SingleOrDefault(x => x.Sid == BOSS_LOCATION_SID);
            int targetCount;
            if (IsBossAvailable(biome, globeLevel) && bossLocation is not null && !biome.IsComplete)
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

        public void CreateCombatsInBiomeNodes(IEnumerable<Biome> biomes, GlobeLevel globeLevel)
        {
            foreach (var biome in biomes)
            {
                var availableNodes = biome.Nodes.Where(x => x.IsAvailable).ToArray();
                Debug.Assert(availableNodes.Any(), "At least of one node expected to be available.");
                var nodesWithCombats = RollNodesWithCombats(biome, _dice, availableNodes, globeLevel);

                var combatCounts = GetCombatSequenceLength(globeLevel.Level);
                var combatLevelAdditionalList = new[]
                {
                    0, -1, 3
                };
                var selectedNodeCombatCount = _dice.RollFromList(combatCounts, 3).ToArray();
                var combatLevelAdditional = 0;

                var combatToTrainingIndex = _dice.RollArrayIndex(nodesWithCombats);

                var globeContext = new MonsterGenerationGlobeContext(globeLevel, biomes);

                for (var locationIndex = 0; locationIndex < nodesWithCombats.Length; locationIndex++)
                {
                    var selectedNode = nodesWithCombats[locationIndex];
                    var targetCombatSenquenceLength = selectedNode.Item2 ? 1 : selectedNodeCombatCount[locationIndex];

                    var combatLevel = globeLevel.Level + combatLevelAdditionalList[combatLevelAdditional];
                    var combatList = new List<CombatSource>();
                    for (var combatIndex = 0; combatIndex < targetCombatSenquenceLength; combatIndex++)
                    {
                        var units = MonsterGeneratorHelper
                            .CreateMonsters(selectedNode.Item1, _dice, combatLevel, _unitSchemeCatalog, globeContext)
                            .ToArray();

                        var combat = new CombatSource
                        {
                            Level = combatLevel,
                            EnemyGroup = new Group(),
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
        }

        public void CreateStartCombat(Biome startBiome)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Biome> GenerateStartState()
        {
            const int BIOME_NODE_COUNT = 4;

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
                            IsLast = x == BIOME_NODE_COUNT - 1
                        }
                    ).ToArray()
                }
            };
        }
    }
}