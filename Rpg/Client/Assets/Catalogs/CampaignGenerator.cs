using System.Collections.Generic;
using System.Linq;
using Client.Assets.StageItems;
using Rpg.Client.Assets.StageItems;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Assets.Catalogs
{
    internal class CampaignGenerator : ICampaignGenerator
    {
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;
        private readonly GlobeProvider _globeProvider;
        private readonly IDice _dice;

        public CampaignGenerator(IUnitSchemeCatalog unitSchemeCatalog, GlobeProvider globeProvider, IDice dice)
        {
            _unitSchemeCatalog = unitSchemeCatalog;
            _globeProvider = globeProvider;
            _dice = dice;
        }

        public IReadOnlyList<HeroCampaign> CreateSet()
        {
            var list = new List<HeroCampaign>();
            for (var i = 0; i < 3; i++)
            {
                var campaign = CreateCampaign();

                list.Add(campaign);
            }

            return list;
        }

        private HeroCampaign CreateCampaign()
        {
            var stages = new List<CampaignStage>();
            for (var i = 0; i < 20; i++)
            {
                var stage = CreateStage(i);
                stages.Add(stage);
            }

            var rewardStageItem = new RewardStageItem(this);
            var rewardStage = new CampaignStage
            {
                Items = new[]
                {
                    rewardStageItem
                }
            };
            stages.Add(rewardStage);

            var campaign = new HeroCampaign
            {
                CampaignStages = stages
            };            

            return campaign;
        }

        private CampaignStage CreateStage(int stageIndex)
        {
            if (stageIndex == 1)
            {
                return CreateTrainingStage();
            }

            return CreateCombatStage();
        }

        private CampaignStage CreateTrainingStage()
        {
            var stage = new CampaignStage
            {
                Items = new[]
                {
                    new TrainingStageItem(_globeProvider.Globe.Player, _dice)
                }
            };

            return stage;
        }

        private CampaignStage CreateCombatStage()
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

            var stageItem = new CombatStageItem(new GlobeNode
            {
                BiomeType = BiomeType.Slavic,
                Sid = GlobeNodeSid.Thicket,
                AssignedCombats = combatSequence
            }, combatSequence, this);

            var monsterInfos = GetStartMonsterInfoList();

            for (var slotIndex = 0; slotIndex < monsterInfos.Count; slotIndex++)
            {
                var scheme = _unitSchemeCatalog.AllMonsters.Single(x => x.Name == monsterInfos[slotIndex].name);
                combat.EnemyGroup.Slots[slotIndex].Unit = new Unit(scheme, monsterInfos[slotIndex].level);
            }

            var stage = new CampaignStage
            {
                Items = new[]
                {
                    new CombatStageItem(
                        new GlobeNode
                        {
                            BiomeType = BiomeType.Slavic,
                            Sid = GlobeNodeSid.Thicket,
                            AssignedCombats = combatSequence
                        },
                        combatSequence, this)
                }
            };

            return stage;
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
    }
}
