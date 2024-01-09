using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class UnlockLocationRewardCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;

    public UnlockLocationRewardCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    private ILocationSid[] CalculateAvailableLocations()
    {
        return GameLocations.GetGameLocations()
            .Except(_services.GlobeProvider.Globe.Player.CurrentAvailableLocations).ToArray();
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var lockedLocations = CalculateAvailableLocations();
        if (!lockedLocations.Any())
        {
            return false;
        }

        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var availableLocations = CalculateAvailableLocations();
        var locationToScout = _services.Dice.RollFromList(availableLocations);

        return new UnlockLocationRewardStageItem(_services.GlobeProvider, _services.JobProgressResolver,
            locationToScout);
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