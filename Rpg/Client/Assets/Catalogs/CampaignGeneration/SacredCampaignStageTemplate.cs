using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class SacredCampaignStageTemplate : ICampaignStageTemplate
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStage("Sacred place");
    }
}
