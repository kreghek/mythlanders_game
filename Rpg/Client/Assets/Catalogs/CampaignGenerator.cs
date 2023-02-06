using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;

using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.StageItems;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class CampaignGenerator : ICampaignGenerator
    {
        private readonly IDice _dice;
        private readonly GlobeProvider _globeProvider;
        private readonly IUnitSchemeCatalog _unitSchemeCatalog;

        public CampaignGenerator(IUnitSchemeCatalog unitSchemeCatalog, GlobeProvider globeProvider, IDice dice)
        {
            _unitSchemeCatalog = unitSchemeCatalog;
            _globeProvider = globeProvider;
            _dice = dice;
        }

        private HeroCampaign CreateCampaign(GlobeNodeSid locationSid, int length)
        {
            var stages = new List<CampaignStage>();
            for (var stageIndex = 0; stageIndex < length; stageIndex++)
            {
                var stage = CreateStage(locationSid, stageIndex);
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

        private CampaignStage CreateCombatStage(GlobeNodeSid locationSid)
        {
            var stageItems = new List<ICampaignStageItem>();
            for (var combatIndex = 0; combatIndex < 3; combatIndex++)
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

                var location = new GlobeNode
                {
                    Sid = locationSid,
                    AssignedCombats = combatSequence
                };
                var stageItem = new CombatStageItem(location, combatSequence, this);

                var monsterInfos = GetStartMonsterInfoList(locationSid);

                for (var slotIndex = 0; slotIndex < monsterInfos.Count; slotIndex++)
                {
                    var scheme = _unitSchemeCatalog.AllMonsters.Single(x => x.Name == monsterInfos[slotIndex].name);
                    combat.EnemyGroup.Slots[slotIndex].Unit = new Unit(scheme, monsterInfos[slotIndex].level);
                }

                stageItems.Add(stageItem);
            }

            var stage = new CampaignStage
            {
                Items = stageItems.ToArray()
            };

            return stage;
        }

        private CampaignStage CreateSlidingPuzzlesStage()
        {
            var stage = new CampaignStage
            {
                Items = new[]
                {
                    new SlidingPuzzlesStageItem(_globeProvider, _dice)
                }
            };

            return stage;
        }

        private CampaignStage CreateStage(GlobeNodeSid locationSid, int stageIndex)
        {
            var stageType = stageIndex % 3;

            if (stageType == 1)
            {
                return CreateTrainingStage();
            }

            if (stageType == 2)
            {
                return CreateSlidingPuzzlesStage();
            }

            return CreateCombatStage(locationSid);
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

        private IReadOnlyList<(UnitName name, int level)> GetStartMonsterInfoList(GlobeNodeSid location)
        {
            var availableAllRegularMonsters = _unitSchemeCatalog.AllMonsters.Where(x => !HasPerk<BossMonster>(x, 1));

            var filteredByLocationMonsters = availableAllRegularMonsters.Where(x =>
                (x.LocationSids is null) || (x.LocationSids is not null && x.LocationSids.Contains(location)));

            var availableMonsters = filteredByLocationMonsters.ToList();

            var rolledUnits = new List<UnitScheme>();

            for (var i = 0; i < 3; i++)
            {
                if (!availableMonsters.Any())
                {
                    break;
                }

                var scheme = _dice.RollFromList(availableMonsters, 1).Single();

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
                var unitLevel = 2;
                var unit = new Unit(unitScheme, unitLevel);
                units.Add(unit);
            }

            return rolledUnits.Select(x => (x.Name, 2)).ToArray();
        }

        private static bool HasPerk<TPerk>(UnitScheme unitScheme, int combatLevel)
        {
            var unit = new Unit(unitScheme, combatLevel);
            return unit.Perks.OfType<TPerk>().Any();
        }

        public IReadOnlyList<HeroCampaign> CreateSet()
        {
            var availbleLocations = new[]
            {
                GlobeNodeSid.Thicket,
                GlobeNodeSid.Monastery,
                GlobeNodeSid.ShipGraveyard,
                GlobeNodeSid.Desert
            };

            var campaignLengths = new[] { 6, 12, 24 };

            var selectedLocations = _dice.RollFromList(availbleLocations, 3).ToList();

            var list = new List<HeroCampaign>();
            for (var i = 0; i < selectedLocations.Count; i++)
            {
                var location = selectedLocations[i];
                var length = campaignLengths[i];

                var campaign = CreateCampaign(location, length);

                list.Add(campaign);
            }

            return list;
        }
    }
}