using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class BiomeGenerator : IBiomeGenerator
    {
        private readonly IDice _dice;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private const int BIOME_NODE_COUNT = 8;

        public BiomeGenerator(IDice dice, IUnitSchemeCatalog unitSchemeCatalog)
        {
            _dice = dice;
            _unitSchemeCatalog = unitSchemeCatalog;
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

        private static GlobeNodeSid? GetUnlockNodeSid(int nodeIndex, BiomeType biomeType)
        {
            if ((nodeIndex == BIOME_NODE_COUNT - 1 && biomeType != BiomeType.Cosmos) ||
                (nodeIndex == 2 && biomeType == BiomeType.Cosmos))
            {
                return null;
            }

            var sidIndex = (int)biomeType + nodeIndex + 1;
            var nextSidIndex = sidIndex + 1;
            var sidToUnlock = (GlobeNodeSid)nextSidIndex;
            return sidToUnlock;
        }

        public IReadOnlyList<Biome> Generate()
        {
            const int BIOME_MIN_LEVEL_STEP = 12;

            return new[]
            {
                new Biome(0, BiomeType.Slavic)
                {
                    IsAvailable = true,
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Slavic),
                            Sid = GetNodeSid(x, BiomeType.Slavic),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            UnlockNodeSid = GetUnlockNodeSid(x, BiomeType.Slavic)
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
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Chinese),
                            Sid = GetNodeSid(x, BiomeType.Chinese),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            UnlockNodeSid = GetUnlockNodeSid(x, BiomeType.Chinese)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Egyptian
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 2, BiomeType.Egyptian)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Egyptian),
                            Sid = GetNodeSid(x, BiomeType.Egyptian),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            UnlockNodeSid = GetUnlockNodeSid(x, BiomeType.Egyptian)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Greek
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 3, BiomeType.Greek)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Greek),
                            Sid = GetNodeSid(x, BiomeType.Greek),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            UnlockNodeSid = GetUnlockNodeSid(x, BiomeType.Greek)
                        }
                    ).ToArray(),
                    UnlockBiome = BiomeType.Cosmos
                },
                new Biome(BIOME_MIN_LEVEL_STEP * 4, BiomeType.Cosmos)
                {
                    Nodes = Enumerable.Range(0, BIOME_NODE_COUNT).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, BiomeType.Cosmos),
                            Sid = GetNodeSid(x, BiomeType.Cosmos),
                            IsAvailable = GetStartAvailability(x),
                            IsLast = x == BIOME_NODE_COUNT - 1,
                            UnlockNodeSid = GetUnlockNodeSid(x, BiomeType.Cosmos)
                        }
                    ).ToArray(),
                    IsFinal = true
                }
            };
        }

        public void CreateCombatsInBiomeNodes(IEnumerable<Biome> biomes)
        {
            foreach (var biome in biomes)
            {
                var availableNodes = biome.Nodes.Where(x => x.IsAvailable).ToArray();
                Debug.Assert(availableNodes.Any(), "At least of one node expected to be available.");
                var nodesWithCombats = RollNodesWithCombats(biome, _dice, availableNodes);

                var combatCounts = GetCombatSequenceLength(biome.Level);
                var combatLevelAdditionalList = new[]
                {
                    0, -1, 3
                };
                var selectedNodeCombatCount = _dice.RollFromList(combatCounts, 3).ToArray();
                var combatLevelAdditional = 0;

                var combatToTrainingIndex = _dice.RollArrayIndex(nodesWithCombats);

                for (var locationIndex = 0; locationIndex < nodesWithCombats.Length; locationIndex++)
                {
                    var selectedNode = nodesWithCombats[locationIndex];
                    var targetCombatSenquenceLength = selectedNode.Item2 ? 1 : selectedNodeCombatCount[locationIndex];

                    var combatLevel = biome.Level + combatLevelAdditionalList[combatLevelAdditional];
                    var combatList = new List<CombatSource>();
                    for (var combatIndex = 0; combatIndex < targetCombatSenquenceLength; combatIndex++)
                    {
                        var units = MonsterGeneratorHelper
                            .CreateMonsters(selectedNode.Item1, _dice, biome, combatLevel, _unitSchemeCatalog).ToArray();

                        var combat = new CombatSource
                        {
                            Level = combatLevel,
                            EnemyGroup = new Group(),
                            IsTrainingOnly = combatToTrainingIndex == locationIndex &&
                                             biome.Nodes.Where(x => x.IsAvailable).Count() == 4,
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

        private static (GlobeNode, bool)[] RollNodesWithCombats(Biome biome, IDice dice, IList<GlobeNode> availableNodes)
        {
            const int COMBAT_UNDER_ATTACK_COUNT = 3;
            const GlobeNodeSid BOSS_LOCATION_SID = GlobeNodeSid.Castle;

            var nodeList = new List<(GlobeNode, bool)>(3);
            var bossLocation = availableNodes.SingleOrDefault(x => x.Sid == BOSS_LOCATION_SID);
            int targetCount;
            if (biome.Level >= 10 && bossLocation is not null && !biome.IsComplete)
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
    }
}