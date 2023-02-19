using Client.Assets.StageItems;

using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class TrainingCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly CampaignStageTemplateServices _services;

    public TrainingCampaignStageTemplateFactory(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    public bool CanCreate()
    {
        return true;
    }

    public ICampaignStageItem Create()
    {
        return new TrainingStageItem(_services.GlobeProvider.Globe.Player, _services.Dice);
    }
}