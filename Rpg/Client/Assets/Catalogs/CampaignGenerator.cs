using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using Core.Dices;
using Core.PropDrop;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs;

internal sealed class CampaignGenerator : ICampaignGenerator
{
    private readonly IDice _dice;
    private readonly IDropResolver _dropResolver;
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly CampaignStageTemplateServices _services;

    public CampaignGenerator(GlobeProvider globeProvider,
        IEventCatalog eventCatalog, IDice dice, IJobProgressResolver jobProgressResolver, IDropResolver dropResolver)
    {
        _services = new CampaignStageTemplateServices(eventCatalog, globeProvider, dice);
        _globeProvider = globeProvider;
        _dice = dice;
        _jobProgressResolver = jobProgressResolver;
        _dropResolver = dropResolver;
    }

    private HeroCampaign CreateCampaign(ILocationSid locationSid)
    {
        var shortTemplateFactories = CreateGrindShortTemplate(locationSid);

        // Create campaign way
        var campaignWay = new List<ICampaignStageItem>();
        
        foreach (var templateFactory in shortTemplateFactories)
        {
            var stageItem = templateFactory.Create(campaignWay.ToArray());
            campaignWay.Add(stageItem);
        }

        var rewardStageItem = new RewardStageItem(_globeProvider, _jobProgressResolver, _dropResolver);
        campaignWay.Add(rewardStageItem);

        var campaignGraph = new CampaignGraph<ICampaignStageItem>();
        ICampaignGraphNode<ICampaignStageItem>? prevCampaignNode = null;
        foreach (var campaignStageItem in campaignWay)
        {
            var campaignGraphNode = new CampaignGraphNode<ICampaignStageItem>(campaignStageItem);
            campaignGraph.AddNode(campaignGraphNode);

            if (prevCampaignNode is not null)
            {
                campaignGraph.ConnectNodes(prevCampaignNode, campaignGraphNode);
            }

            prevCampaignNode = campaignGraphNode;
        }

        var campaign = new HeroCampaign(locationSid, campaignGraph);

        return campaign;
    }

    private ICampaignStageTemplateFactory[] CreateGrindShortTemplate(ILocationSid locationSid)
    {
        return new ICampaignStageTemplateFactory[]
        {
            //// To debug text events
            //new ICampaignStageTemplateFactory[]
            //{
            //    new PrioritySelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]{
            //        new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
            //        new ChallengeCampaignStageTemplateFactory(),
            //    })
            //},

            //// To debug crisis
            //new ICampaignStageTemplateFactory[]
            //{
            //    new CrisisEventCampaignStageTemplateFactory()
            //},

            //// Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTempateLevels.Easy, _services),

            // Rest

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new RestCampaignStageTemplateFactory(),
                new ShopCampaignStageTemplateFactory(),
                //new SacredEventCampaignStageTemplateFactory(),
                //new ShopCampaignStageTemplateFactory(),
                new FindingEventCampaignStageTemplateFactory()
            }, _services),
            

            // // Evo
            //
            // new ICampaignStageTemplateFactory[]
            // {
            //     new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            //     {
            //         new TrainingCampaignStageTemplateFactory(_services),
            //         new WorkshopCampaignStageTemplateFactory(_services)
            //     }, _services),
            //
            //     new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            //     {
            //         new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
            //         new ChallengeCampaignStageTemplateFactory(),
            //         new SacredEventCampaignStageTemplateFactory(),
            //         new MinigameEventCampaignStageTemplateFactory()
            //     }, _services)
            // },

            // Crisis

            
            new CrisisEventCampaignStageTemplateFactory(),

            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTempateLevels.Medium, _services),

            // Rest

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new RestCampaignStageTemplateFactory(),
                new ShopCampaignStageTemplateFactory(),
                //new SacredEventCampaignStageTemplateFactory(),
                //new ShopCampaignStageTemplateFactory(),
                new FindingEventCampaignStageTemplateFactory()
            }, _services),

            // For demo only

            new PrioritySelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new MinigameEventCampaignStageTemplateFactory()
            }),

            // Evo

            // new ICampaignStageTemplateFactory[]
            // {
            //     new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            //     {
            //         new TrainingCampaignStageTemplateFactory(_services),
            //         new WorkshopCampaignStageTemplateFactory(_services)
            //     }, _services),
            //
            //     new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            //     {
            //         new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
            //         new SacredEventCampaignStageTemplateFactory(),
            //         new MinigameEventCampaignStageTemplateFactory()
            //     }, _services)
            // },

            // Crisis

            new CrisisEventCampaignStageTemplateFactory(),

            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTempateLevels.Hard, _services)
        };
    }

    public IReadOnlyList<HeroCampaign> CreateSet()
    {
        var availbleLocations = new[]
        {
            LocationSids.Thicket,
            LocationSids.Monastery,
            LocationSids.ShipGraveyard,
            LocationSids.Desert,

            LocationSids.Swamp
        };

        var selectedLocations = _dice.RollFromList(availbleLocations, 3).ToList();

        var list = new List<HeroCampaign>();
        for (var i = 0; i < selectedLocations.Count; i++)
        {
            var location = selectedLocations[i];

            var campaign = CreateCampaign(location);

            list.Add(campaign);
        }

        return list;
    }
}