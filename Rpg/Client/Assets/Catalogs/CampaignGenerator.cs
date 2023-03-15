using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.StageItems;
using Client.Core.Campaigns;

using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs;

internal sealed class CampaignGenerator : ICampaignGenerator
{
    private readonly IDice _dice;
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly CampaignStageTemplateServices _services;

    public CampaignGenerator(IUnitSchemeCatalog unitSchemeCatalog, GlobeProvider globeProvider,
        IEventCatalog eventCatalog, IDice dice, IJobProgressResolver jobProgressResolver)
    {
        _services = new CampaignStageTemplateServices(unitSchemeCatalog, eventCatalog, globeProvider, dice);
        _globeProvider = globeProvider;
        _dice = dice;
        _jobProgressResolver = jobProgressResolver;
    }

    private HeroCampaign CreateCampaign(LocationSid locationSid)
    {
        var shortTemplate = CreateGrindShortTemplate(locationSid);

        var stages = new List<CampaignStage>();
        foreach (var template in shortTemplate)
        {
            var itemList = new List<ICampaignStageItem>();

            foreach (var templateItem in template)
            {
                var currentStageItems = itemList.ToArray();
                var stageItem = templateItem.Create(currentStageItems);
                itemList.Add(stageItem);
            }

            var stage = new CampaignStage
            {
                Items = itemList
            };

            stages.Add(stage);
        }

        var rewardStageItem = new RewardStageItem(this, _globeProvider, _jobProgressResolver);
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

    private ICampaignStageTemplateFactory[][] CreateGrindShortTemplate(LocationSid locationSid)
    {
        return new[]
        {
            // Combat

            new ICampaignStageTemplateFactory[]
            {
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services)
            },

            // Rest

            new ICampaignStageTemplateFactory[]
            {
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new RestCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory()
                }, _services),
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new SacredEventCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory(),
                    new FindingEventCampaignStageTemplateFactory()
                }, _services)
            },

            // Evo

            new ICampaignStageTemplateFactory[]
            {
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new TrainingCampaignStageTemplateFactory(_services),
                    new WorkshopCampaignStageTemplateFactory(_services)
                }, _services),

                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
                    new ChallengeCampaignStageTemplateFactory(),
                    new SacredEventCampaignStageTemplateFactory(),
                    new MinigameEventCampaignStageTemplateFactory()
                }, _services)
            },

            // Crisis

            new ICampaignStageTemplateFactory[]
            {
                new CrisisEventCampaignStageTemplateFactory()
            },

            // Combat

            new ICampaignStageTemplateFactory[]
            {
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services),
                new CombatCampaignStageTemplateFactory(locationSid, _services)
            },

            // Rest

            new ICampaignStageTemplateFactory[]
            {
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new RestCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory()
                }, _services),
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new SacredEventCampaignStageTemplateFactory(),
                    new ShopCampaignStageTemplateFactory(),
                    new FindingEventCampaignStageTemplateFactory()
                }, _services)
            },

            // Evo

            new ICampaignStageTemplateFactory[]
            {
                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new TrainingCampaignStageTemplateFactory(_services),
                    new WorkshopCampaignStageTemplateFactory(_services)
                }, _services),

                new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
                    new SacredEventCampaignStageTemplateFactory(),
                    new MinigameEventCampaignStageTemplateFactory()
                }, _services)
            },

            // Crisis

            new ICampaignStageTemplateFactory[]
            {
                new CrisisEventCampaignStageTemplateFactory()
            },

            // Combat

            new ICampaignStageTemplateFactory[]
            {
                new CombatCampaignStageTemplateFactory(locationSid, _services)
            }
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