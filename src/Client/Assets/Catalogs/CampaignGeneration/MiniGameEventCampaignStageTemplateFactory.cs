using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class MiniGameEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;

    public MiniGameEventCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return _allMiniGameFeatures.Any(x => _services.GlobeProvider.Globe.Features.HasFeature(x));
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var rolledMiniGameFeature = RollMiniGameFeature();

        if (rolledMiniGameFeature == GameFeatures.SlidingPuzzleMiniGame)
        {
            return new SlidingPuzzlesMiniGameStageItem();
        }

        if (rolledMiniGameFeature == GameFeatures.TowersMiniGame)
        {
            return new TowersMinigameStageItem();
        }

        if (rolledMiniGameFeature == GameFeatures.Match3MiniGame)
        {
            return new Match3MinigameStageItem();
        }

        // rollback
        return new SlidingPuzzlesMiniGameStageItem();
    }

    private readonly IReadOnlyCollection<GameFeature> _allMiniGameFeatures = new[]
    {
        GameFeatures.SlidingPuzzleMiniGame, GameFeatures.Match3MiniGame, GameFeatures.TowersMiniGame
    };

    private GameFeature RollMiniGameFeature()
    {
        var availableMiniGames = FilterMiniGameFeatures(_allMiniGameFeatures);

        var rolledMiniGameFeatures = _services.Dice.RollFromList(availableMiniGames.ToArray());

        return rolledMiniGameFeatures;
    }

    private IEnumerable<GameFeature> FilterMiniGameFeatures(IEnumerable<GameFeature> allMiniGameFeatures)
    {
        return allMiniGameFeatures.Where(x => _services.GlobeProvider.Globe.Features.HasFeature(x)).ToArray();
    }

    /// <inheritdoc />
    public IGraphNode<ICampaignStageItem> Create(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return new GraphNode<ICampaignStageItem>(Create(MapContextToCurrentStageItems(context)));
    }

    /// <inheritdoc />
    public bool CanCreate(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return CanCreate(MapContextToCurrentStageItems(context));
    }
}