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

    public CampaignGenerator(IUnitSchemeCatalog unitSchemeCatalog, GlobeProvider globeProvider, IEventCatalog eventCatalog, IDice dice)
    {
        _services = new CampaignStageTemplateServices(unitSchemeCatalog, eventCatalog, globeProvider, dice);
        _dice = dice;
    }

    private HeroCampaign CreateCampaign(GlobeNodeSid locationSid, int length)
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

    private ICampaignStageTemplate[][] CreateShortTemplate(GlobeNodeSid locationSid)
    {
        return new ICampaignStageTemplate[][] {
            // Combat

            new ICampaignStageTemplate[]{
                new CombatCampaignStageTemplate(locationSid, _services),
                new CombatCampaignStageTemplate(locationSid, _services),
                new CombatCampaignStageTemplate(locationSid, _services),
            },

            // Rest

            new ICampaignStageTemplate[]{
                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{ 
                    new RestCampaignStageTemplate(),
                    new ShopCampaignStageTemplate()
                }, _services),
                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new SacredCampaignStageTemplate(),
                    new ShopCampaignStageTemplate(),
                    new FindingCampaignStageTemplate()
                }, _services),

            },

            // Evo

            new ICampaignStageTemplate[]{
                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new TrainingCampaignStageTemplate(_services),
                    new WorkshopCampaignStageTemplate(_services)
                    }, _services),

                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new SideQuestStageTemplate(locationSid, _services),
                    new SacredCampaignStageTemplate(),
                    new MinigameCampaignStageTemplate()
                }, _services)
            },

            // Crisis

            new ICampaignStageTemplate[]{
                new CrisisCampaignStageTemplate()
            },

            // Combat

            new ICampaignStageTemplate[]{
                new CombatCampaignStageTemplate(locationSid, _services),
                new CombatCampaignStageTemplate(locationSid, _services),
                new CombatCampaignStageTemplate(locationSid, _services),
            },

            // Rest

            new ICampaignStageTemplate[]{
                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new RestCampaignStageTemplate(),
                    new ShopCampaignStageTemplate()
                }, _services),
                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new SacredCampaignStageTemplate(),
                    new ShopCampaignStageTemplate(),
                    new FindingCampaignStageTemplate()
                }, _services),

            },

            // Evo

            new ICampaignStageTemplate[]{
                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new TrainingCampaignStageTemplate(_services),
                    new WorkshopCampaignStageTemplate(_services)
                    }, _services),

                new RandomSelectCampaignStageTemplate(new ICampaignStageTemplate[]{
                    new SideQuestStageTemplate(locationSid, _services),
                    new SacredCampaignStageTemplate(),
                    new MinigameCampaignStageTemplate()
                }, _services)
            },

            // Crisis

            new ICampaignStageTemplate[]{
                new CrisisCampaignStageTemplate()
            },

            // Combat

            new ICampaignStageTemplate[]{
                new CombatCampaignStageTemplate(locationSid, _services)
            },
        };
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