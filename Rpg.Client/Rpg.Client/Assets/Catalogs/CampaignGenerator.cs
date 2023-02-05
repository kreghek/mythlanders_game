using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.StageItems;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Assets.Catalogs
{
    internal class CampaignGenerator : ICampaignGenerator
    {
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public CampaignGenerator(IUnitSchemeCatalog unitSchemeCatalog)
        {
            _unitSchemeCatalog = unitSchemeCatalog;
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
            for (var i = 0; i < 3; i++)
            {
                var stage = CreateStage();
                stages.Add(stage);
            }

            var campaign = new HeroCampaign
            {
                CampaignStages = stages
            };            

            return campaign;
        }

        private CampaignStage CreateStage()
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
            }, combatSequence);

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
                        combatSequence)
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
