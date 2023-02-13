using Core.Dices;
using System.Collections.Generic;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class RandomSelectCampaignStageTemplate : ICampaignStageTemplate
{
    private readonly IReadOnlyList<ICampaignStageTemplate> _templates;
    private readonly CampaignStageTemplateServices _services;

    public RandomSelectCampaignStageTemplate(IReadOnlyList<ICampaignStageTemplate> templates, CampaignStageTemplateServices services)
    {
        _templates = templates;
        _services = services;
    }

    public ICampaignStageItem Create()
    {
        return _services.Dice.RollFromList(_templates).Create();
    }
}
