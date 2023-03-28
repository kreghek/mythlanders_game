using System.Collections.Generic;

using Client.Assets.StageItems;
using Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class TrainingCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;

    public TrainingCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return new TrainingStageItem(_services.GlobeProvider.Globe.Player, _services.Dice);
    }
}