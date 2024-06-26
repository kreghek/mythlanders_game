using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.Catalogs.Crises;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.PropDrop;

namespace Client.Assets.Catalogs;

internal sealed class CampaignWayTemplatesCatalog
{
    private readonly CampaignStageTemplateServices _services;

    public CampaignWayTemplatesCatalog(GlobeProvider globeProvider,
        IEventCatalog eventCatalog, IDice dice, IJobProgressResolver jobProgressResolver, IDropResolver dropResolver,
        ICharacterCatalog unitSchemeCatalog, ICrisesCatalog crisesCatalog, IMonsterPerkManager monsterPerkManager)
    {
        _services = new CampaignStageTemplateServices(eventCatalog, globeProvider, jobProgressResolver, dropResolver,
            dice, unitSchemeCatalog, crisesCatalog, monsterPerkManager);
    }

    public IGraph<GraphWay<ICampaignStageItem>> CreateGrindShortTemplate(ILocationSid locationSid)
    {
        if (_services.GlobeProvider.Globe.Features.HasFeature(GameFeatures.Shop) &&
            _services.GlobeProvider.Globe.Features.HasFeature(GameFeatures.SmallEvents) &&
            _services.GlobeProvider.Globe.Features.HasFeature(GameFeatures.Craft) &&
            _services.GlobeProvider.Globe.Features.HasFeature(GameFeatures.SacredPlace))
        {
            return CreateShortTemplate(locationSid);
        }

        return CreateShortDemoTemplate(locationSid);
    }

    /// <summary>
    /// Creates graph
    /// </summary>
    private IGraph<GraphWay<ICampaignStageItem>> CreateShortDemoTemplate(ILocationSid locationSid)
    {
        var wayGraph = new DirectedGraph<GraphWay<ICampaignStageItem>>();

        var way1Templates = new ICampaignStageTemplateFactory[]
        {
            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Easy, _services),

            // Rest

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new RestCampaignStageTemplateFactory(_services)
            }, _services)
        };

        var way2Templates = new ICampaignStageTemplateFactory[]
        {
            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Easy, _services),

            // Rest

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new RestCampaignStageTemplateFactory(_services)
            }, _services)
        };

        var way3Templates = new ICampaignStageTemplateFactory[]
        {
            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Medium, _services)
        };

        var regular1Way = new GraphWay<ICampaignStageItem>(way1Templates);
        var way11Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular1Way);
        var way12Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular1Way);
        var way13Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular1Way);

        var regular2Way = new GraphWay<ICampaignStageItem>(way2Templates);
        var way2Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular2Way);

        var regular3Way = new GraphWay<ICampaignStageItem>(way3Templates);
        var way31Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular3Way);
        var way32Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular3Way);

        var rewardNode = new GraphNode<GraphWay<ICampaignStageItem>>(new GraphWay<ICampaignStageItem>(new[]
        {
            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Hard, _services)
            {
                IsGoalStage = true
            }
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

    /// <summary>
    /// Creates graph
    /// </summary>
    private IGraph<GraphWay<ICampaignStageItem>> CreateShortTemplate(ILocationSid locationSid)
    {
        var wayGraph = new DirectedGraph<GraphWay<ICampaignStageItem>>();

        var way1Templates = new ICampaignStageTemplateFactory[]
        {
            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Easy, _services),

            // Rest

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new RestCampaignStageTemplateFactory(_services),
                new ShopCampaignStageTemplateFactory(),
                new FindingEventCampaignStageTemplateFactory(_services),
                new ChallengeCampaignStageTemplateFactory(_services)
            }, _services),

            // Crisis

            new CrisisEventCampaignStageTemplateFactory(_services)
        };

        var way2Templates = new ICampaignStageTemplateFactory[]
        {
            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Medium, _services),

            // Rest

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new RestCampaignStageTemplateFactory(_services),
                new ShopCampaignStageTemplateFactory(),
                //new SacredEventCampaignStageTemplateFactory(),
                //new ShopCampaignStageTemplateFactory(),
                new FindingEventCampaignStageTemplateFactory(_services),
                new ChallengeCampaignStageTemplateFactory(_services)
            }, _services),

            // Crisis

            new CrisisEventCampaignStageTemplateFactory(_services)
        };

        var way3Templates = new ICampaignStageTemplateFactory[]
        {
            // Evo

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new TrainingCampaignStageTemplateFactory(_services),
                new WorkshopCampaignStageTemplateFactory(_services)
            }, _services),

            new RandomSelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
            {
                new SideStoryDialogueEventStageTemplateFactory(locationSid, _services),
                new SacredEventCampaignStageTemplateFactory(),
                new PrioritySelectCampaignStageTemplateFactory(new ICampaignStageTemplateFactory[]
                {
                    new MiniGameEventCampaignStageTemplateFactory(_services),
                    new RestCampaignStageTemplateFactory(_services)
                })
            }, _services),

            // Combat

            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Hard, _services)
        };

        var regular1Way = new GraphWay<ICampaignStageItem>(way1Templates);
        var way11Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular1Way);
        var way12Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular1Way);
        var way13Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular1Way);

        var regular2Way = new GraphWay<ICampaignStageItem>(way2Templates);
        var way2Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular2Way);

        var regular3Way = new GraphWay<ICampaignStageItem>(way3Templates);
        var way31Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular3Way);
        var way32Node = new GraphNode<GraphWay<ICampaignStageItem>>(regular3Way);

        var rewardNode = new GraphNode<GraphWay<ICampaignStageItem>>(new GraphWay<ICampaignStageItem>(new[]
        {
            new CombatCampaignStageTemplateFactory(locationSid, MonsterCombatantTemplateLevels.Hard, _services)
            {
                IsGoalStage = true
            }
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