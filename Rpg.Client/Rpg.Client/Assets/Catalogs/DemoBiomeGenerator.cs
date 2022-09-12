using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Rpg.Client.Assets.StageItems;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DemoBiomeGenerator : IBiomeGenerator
    {
        public static GlobeNodeSid START_AVAILABLE_LOCATION = GlobeNodeSid.Thicket;
        private readonly IDice _dice;
        private readonly IEventCatalog _eventCatalog;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public DemoBiomeGenerator(IDice dice, IUnitSchemeCatalog unitSchemeCatalog, IEventCatalog eventCatalog)
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
                        GlobeNodeSid.Thicket,
                        GlobeNodeSid.Battleground,
                        GlobeNodeSid.DestroyedVillage,
                        GlobeNodeSid.Swamp
                    }
                },
                {
                    BiomeType.Chinese,
                    new[]
                    {
                        GlobeNodeSid.Monastery
                    }
                },
                {
                    BiomeType.Egyptian,
                    new[]
                    {
                        GlobeNodeSid.Desert
                    }
                },
                {
                    BiomeType.Greek,
                    new[]
                    {
                        GlobeNodeSid.ShipGraveyard
                    }
                }
            };

            return demoLocationsDict[biomeType][nodeIndex];
        }

        private static bool GetStartAvailability(GlobeNodeSid currentLocationSid)
        {
            return currentLocationSid == START_AVAILABLE_LOCATION;
        }

        private static IReadOnlyList<(UnitName name, int level)> GetStartMonsterInfoList()
        {
            return new (UnitName name, int level)[]
            {
                new(UnitName.DigitalWolf, 2),
                new(UnitName.BoldMarauder, 2),
                new(UnitName.BlackTrooper, 2)
            };
        }

        private static bool IsBossAvailable(GlobeLevel globeLevel)
        {
            return globeLevel.Level >= 5;
        }

        private static (GlobeNode Location, bool IsBoss)[] RollNodesWithCombats(Biome biome, IDice dice,
            IList<GlobeNode> availableNodes, GlobeLevel globeLevel)
        {
            const int COMBAT_UNDER_ATTACK_COUNT = 3;
            const GlobeNodeSid BOSS_LOCATION_SID = GlobeNodeSid.Swamp;

            var nodeList = new List<(GlobeNode, bool)>(3);
            var bossLocation = availableNodes.SingleOrDefault(x => x.Sid == BOSS_LOCATION_SID);
            int targetCount;
            if (IsBossAvailable(globeLevel) && bossLocation is not null && !biome.IsComplete)
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
                if (!availableNodes.Any())
                {
                    continue;
                }

                var nodesWithCombatTuples = RollNodesWithCombats(biome, _dice, availableNodes, globeLevel);

                var combatCounts = GetCombatSequenceLength(globeLevel.Level);
                var combatLevelAdditionalList = new[]
                {
                    0, -1, 3
                };
                var selectedNodeCombatCount = _dice.RollFromList(combatCounts, 3).ToArray();
                var combatLevelAdditional = 0;

                var combatToTrainingIndex = _dice.RollArrayIndex(nodesWithCombatTuples);

                var globeContext = new MonsterGenerationGlobeContext(globeLevel, biomes);

                for (var locationIndex = 0; locationIndex < nodesWithCombatTuples.Length; locationIndex++)
                {
                    var selectedNodeTuple = nodesWithCombatTuples[locationIndex];
                    var targetCombatSenquenceLength = selectedNodeTuple.IsBoss ? 1 : selectedNodeCombatCount[locationIndex];

                    var combatLevel = globeLevel.Level + combatLevelAdditionalList[combatLevelAdditional];
                    var combatList = new List<CombatSource>();
                    for (var combatIndex = 0; combatIndex < targetCombatSenquenceLength; combatIndex++)
                    {
                        var units = MonsterGeneratorHelper
                            .CreateMonsters(selectedNodeTuple.Location, _dice, combatLevel, _unitSchemeCatalog, globeContext)
                            .ToArray();

                        var combat = new CombatSource
                        {
                            Level = combatLevel,
                            EnemyGroup = new Group(),
                            IsBossLevel = selectedNodeTuple.IsBoss
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

                    var campaign = new HeroCampaign
                    {
                        CampaignStages = Enumerable.Range(0, 10).Select(x =>
                            new CampaignStage
                            {
                                Items = Enumerable.Range(0, _dice.Roll(3))
                                .Select(y => new CombatStageItem(selectedNodeTuple.Location, combatSequence)).ToArray()
                            }
                        ).ToArray()
                    };

                    selectedNodeTuple.Location.AssignedCombats = combatSequence;

                    combatLevelAdditional++;
                }
            }
        }

        public void CreateStartCombat(Globe globe)
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

            var startNode = globe.Biomes.SelectMany(x => x.Nodes).Single(x => x.Sid == START_AVAILABLE_LOCATION);

            var startEvent = _eventCatalog.Events.Single(x => x.IsGameStart);
            var startDialogue = _eventCatalog.GetDialogue(startEvent.BeforeCombatStartNodeSid);

            var campaign = new HeroCampaign
            {
                CampaignStages = new[]
                {
                    new CampaignStage
                    {
                        Items = new ICampaignStageItem[]
                        {
                            new CombatStageItem(startNode, combatSequence)
                        }
                    },
                    new CampaignStage
                    {
                        Items = new ICampaignStageItem[]
                        {
                            new CombatStageItem(startNode, combatSequence), new CombatStageItem(startNode, combatSequence), new CombatStageItem(startNode, combatSequence)
                        }
                    },
                    new CampaignStage
                    {
                        Items = new ICampaignStageItem[]
                        {
                            new CombatStageItem(startNode, combatSequence)
                        }
                    }
                }
            };

            startNode.Campaign = campaign;

            startNode.AssignedCombats = combatSequence;
            startNode.AssignEvent(startEvent);
            var monsterInfos = GetStartMonsterInfoList();

            for (var slotIndex = 0; slotIndex < monsterInfos.Count; slotIndex++)
            {
                var scheme = _unitSchemeCatalog.AllMonsters.Single(x => x.Name == monsterInfos[slotIndex].name);
                combat.EnemyGroup.Slots[slotIndex].Unit = new Unit(scheme, monsterInfos[slotIndex].level);
            }
        }

        public IReadOnlyList<Biome> GenerateStartState()
        {
            const int SLAVIC_BIOME_NODE_COUNT = 4;
            const int CHINESE_BIOME_NODE_COUNT = 1;
            const int EGYPTIAN_BIOME_NODE_COUNT = 1;
            const int GREEK_BIOME_NODE_COUNT = 1;

            Biome CreateBiome(BiomeType type, int nodeCount)
            {
                return new Biome(type)
                {
                    Nodes = Enumerable.Range(0, nodeCount).Select(x =>
                        new GlobeNode
                        {
                            EquipmentItem = GetEquipmentItem(x, type),
                            Sid = GetNodeSid(x, type),
                            BiomeType = type,
                            IsAvailable = GetStartAvailability(GetNodeSid(x, type)),
                            IsLast = x == nodeCount - 1
                        }
                    ).ToArray()
                };
            }

            return new[]
            {
                CreateBiome(BiomeType.Slavic, SLAVIC_BIOME_NODE_COUNT),
                CreateBiome(BiomeType.Chinese, CHINESE_BIOME_NODE_COUNT),
                CreateBiome(BiomeType.Egyptian, EGYPTIAN_BIOME_NODE_COUNT),
                CreateBiome(BiomeType.Greek, GREEK_BIOME_NODE_COUNT)
            };
        }
    }
}