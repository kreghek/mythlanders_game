using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class ShopCampaignStageTemplate : ICampaignStageTemplate
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStage("Shop");
    }
}
