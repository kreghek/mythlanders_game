using System;
using System.Collections.Generic;
using System.Linq;

using Core.Dices;

using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Wrapper of stage factories to select one of the templates from list randomly.
/// </summary>
internal sealed class RandomSelectCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;
    private readonly IReadOnlyList<ICampaignStageTemplateFactory> _templates;

    public RandomSelectCampaignStageTemplateFactory(IReadOnlyList<ICampaignStageTemplateFactory> templates,
        CampaignStageTemplateServices services)
    {
        _templates = templates;
        _services = services;
    }

    /// <inheritdoc />
    public bool CanCreate()
    {
        return true;
    }

    /// <inheritdoc/>
    public ICampaignStageItem Create()
    {
        var openList = new List<ICampaignStageTemplateFactory>(_templates);

        while (openList.Any())
        {
            var rolledTemplate = _services.Dice.RollFromList(openList);
            if (rolledTemplate.CanCreate())
            {
                return rolledTemplate.Create();
            }
            else
            { 
                openList.Remove(rolledTemplate);
            }
        }

        
        throw new InvalidOperationException("Can't select template to create stage.");
    }
}