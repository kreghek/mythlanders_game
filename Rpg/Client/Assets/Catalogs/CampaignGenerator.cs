using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.StageItems;

using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs;

internal sealed class CampaignGenerator : ICampaignGenerator
{
    private readonly CampaignStageTemplateServices _services;
    private readonly IDice _dice;

    public CampaignGenerator(IUnitSchemeCatalog unitSchemeCatalog, GlobeProvider globeProvider,
        IEventCatalog eventCatalog, IDice dice)
    {
        _services = new CampaignStageTemplateServices(unitSchemeCatalog, eventCatalog, globeProvider, dice);
        _dice = dice;
    }

    private HeroCampaign CreateCampaign(LocationSid locationSid)
    {
        var shortTemplate = CreateShortTemplate(locationSid);

        var stages = new List<CampaignStage>();
        for (var stageIndex = 0; stageIndex < shortTemplate.Length; stageIndex++)
        {
            var itemList = new List<ICampaignStageItem>();

            for (int stageItemIndex = 0; stageItemIndex < shortTemplate[stageIndex].Length; stageItemIndex++)
            {
                var stageItem = shortTemplate[stageIndex][stageItemIndex].Create();
                itemList.Add(stageItem);
            }

            var stage = new CampaignStage
            {
                Items = itemList
            };

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

        var campaign = new HeroCampaign(locationSid, stages);

        return campaign;
    }

    private ICampaignStageTemplateFactory[][] CreateShortTemplate(LocationSid locationSid)
    {
        return new ICampaignStageTemplateFactory[][] {
            // Combat

            new ICampaignStageTemplateFactory[]{
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services),
            },

            // Rest

            new ICampaignStageTemplateFactory[]{
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new RestCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory()
                }, _services),
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new SacredEventCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory(),
                    new FindingEventCampaignStageTemplateFactory()
                }, _services),

            },

            // Evo

            new ICampaignStageTemplateFactory[]{
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new TrainingCampaignStageTemplateFactory(_services),
                    new WorkshopCampaignStageTemplateFactory(_services)
                    }, _services),

                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
                    new SacredEventCampaignStageTemplateFactory(),
                    new MinigameEventCampaignStageTemplateFactory()
                }, _services)
            },

            // Crisis

            new ICampaignStageTemplateFactory[]{
                new CrisisEventCampaignStageTemplateFactory()
            },

            // Combat

            new ICampaignStageTemplateFactory[]{
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services),
            },

            // Rest

            new ICampaignStageTemplateFactory[]{
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new RestCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory()
                }, _services),
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new SacredEventCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory(),
                    new FindingEventCampaignStageTemplateFactory()
                }, _services),

            },

            // Evo

            new ICampaignStageTemplateFactory[]{
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new TrainingCampaignStageTemplateFactory(_services),
                    new WorkshopCampaignStageTemplateFactory(_services)
                    }, _services),

                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
                    new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
                    new SacredEventCampaignStageTemplateFactory(),
                    new MinigameEventCampaignStageTemplateFactory()
                }, _services)
            },

            // Crisis

            new ICampaignStageTemplateFactory[]{
                new CrisisEventCampaignStageTemplateFactory()
            },

            // Combat

            new ICampaignStageTemplateFactory[]{
                new CombatCampaignStageTemplateFactory(locationSid, _services)
            },
        };
    }

    public IReadOnlyList<HeroCampaign> CreateSet()
    {
        var availbleLocations = new[]
        {
            LocationSid.Thicket,
            LocationSid.Monastery,
            LocationSid.ShipGraveyard,
            LocationSid.Desert
        };

        var campaignLengths = new[] { 6, 12, 24 };

        var selectedLocations = _dice.RollFromList(availbleLocations, 3).ToList();

        var list = new List<HeroCampaign>();
        for (var i = 0; i < selectedLocations.Count; i++)
        {
            var location = selectedLocations[i];
            var length = campaignLengths[i];

            var campaign = CreateCampaign(location);

            list.Add(campaign);
        }

        return list;
    }
}