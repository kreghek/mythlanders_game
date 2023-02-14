using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class SacredEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStageItem("Sacred place");
    }
}
