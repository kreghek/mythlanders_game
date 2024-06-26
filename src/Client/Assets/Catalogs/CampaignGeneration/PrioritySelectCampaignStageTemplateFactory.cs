﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Campaigns;

using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Wrapper of stage factories to select one of the templates from list sequentialy.
/// </summary>
internal sealed class PrioritySelectCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly IReadOnlyList<ICampaignStageTemplateFactory> _templates;

    public PrioritySelectCampaignStageTemplateFactory(IReadOnlyList<ICampaignStageTemplateFactory> templates)
    {
        _templates = templates;
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    /// <inheritdoc />
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    /// <inheritdoc />
    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        foreach (var template in _templates)
        {
            if (template.CanCreate(currentStageItems))
            {
                return template.Create(currentStageItems);
            }
        }

        throw new InvalidOperationException("Can't select template to create stage.");
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