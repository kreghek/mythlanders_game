using Core.Dices;
using System.Collections.Generic;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Wrapper of stage factories to select one of the templates from list randomly.
/// </summary>
internal sealed class RandomSelectCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly IReadOnlyList<ICampaignStageTemplateFactory> _templates;
    private readonly CampaignStageTemplateServices _services;

    public RandomSelectCampaignStageTemplateFactory(IReadOnlyList<ICampaignStageTemplateFactory> templates, CampaignStageTemplateServices services)
    {
        _templates = templates;
        _services = services;
    }

    /// <inheritdoc/>
    public ICampaignStageItem Create()
    {
        return _services.Dice.RollFromList(_templates).Create();
    }
}
