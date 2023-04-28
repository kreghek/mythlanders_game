using Client.Assets.Catalogs.CampaignGeneration;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.Dices;
using Core.PropDrop;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs;

internal sealed class CampaignWayTemplatesCatalog
{
    private readonly CampaignStageTemplateServices _services;

    public CampaignWayTemplatesCatalog(GlobeProvider globeProvider,
        IEventCatalog eventCatalog, IDice dice, IJobProgressResolver jobProgressResolver, IDropResolver dropResolver)
    {
        _services = new CampaignStageTemplateServices(eventCatalog, globeProvider, jobProgressResolver, dropResolver, dice);
    }

    /// <summary>
    /// Creates graph
    /// </summary>
    public IGraph<GraphWay<ICampaignStageItem>> CreateGrindShortTemplate(ILocationSid locationSid)
    {
        var wayGraph = new Graph<GraphWay<ICampaignStageItem>>();

        var wayTemplates = new ICampaignStageTemplateFactory[]
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
        
        var regularWay = new GraphWay<ICampaignStageItem>(wayTemplates);
        var way11Node = new GraphNode<GraphWay<ICampaignStageItem>>(regularWay);
        var way12Node = new GraphNode<GraphWay<ICampaignStageItem>>(regularWay);
        var way13Node = new GraphNode<GraphWay<ICampaignStageItem>>(regularWay);
        
        var way2Node = new GraphNode<GraphWay<ICampaignStageItem>>(regularWay);
        
        var way31Node = new GraphNode<GraphWay<ICampaignStageItem>>(regularWay);
        var way32Node = new GraphNode<GraphWay<ICampaignStageItem>>(regularWay);
        
        var rewardNode = new GraphNode<GraphWay<ICampaignStageItem>>(new GraphWay<ICampaignStageItem>(new[]
        {
            new RewardCampaignStageTemplateFactory(_services)
        }));
        
        wayGraph.AddNode(way11Node);
        wayGraph.AddNode(way12Node);
        wayGraph.AddNode(way13Node);
        
        wayGraph.AddNode(way2Node);
        
        wayGraph.ConnectNodes(way11Node, way2Node);
        wayGraph.ConnectNodes(way12Node, way2Node);
        wayGraph.ConnectNodes(way13Node, way2Node);
        
        wayGraph.AddNode(way31Node);
        wayGraph.AddNode(way32Node);
        
        wayGraph.ConnectNodes(way2Node, way31Node);
        wayGraph.ConnectNodes(way2Node, way32Node);
        
        wayGraph.AddNode(rewardNode);
        
        wayGraph.ConnectNodes(way31Node, rewardNode);
        wayGraph.ConnectNodes(way32Node, rewardNode);
        
        return wayGraph;
    }
}