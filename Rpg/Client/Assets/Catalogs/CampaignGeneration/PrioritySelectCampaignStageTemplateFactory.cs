using System;
using System.Collections.Generic;

using Rpg.Client.Core.Campaigns;

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
}