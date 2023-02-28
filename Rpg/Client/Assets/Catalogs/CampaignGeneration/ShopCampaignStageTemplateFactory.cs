using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class ShopCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public bool CanCreate(System.Collections.Generic.IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(System.Collections.Generic.IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return new NotImplemenetedStageItem("Shop");
    }
}