using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class ShopCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public bool CanCreate()
    {
        return true;
    }

    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStageItem("Shop");
    }
}