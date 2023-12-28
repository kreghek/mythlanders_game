using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Crises;
using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class FindingEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly ICrisesCatalog _crisesCatalog;
    private readonly IDice _dice;
    private readonly IEventCatalog _eventCatalog;

    public FindingEventCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _dice = services.Dice;
        _crisesCatalog = services.CrisesCatalog;
        _eventCatalog = services.EventCatalog;
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return new FindingStageItem(_dice, _crisesCatalog, _eventCatalog);
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