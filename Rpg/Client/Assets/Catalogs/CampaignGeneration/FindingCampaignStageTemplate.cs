using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class FindingCampaignStageTemplate : ICampaignStageTemplate
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStage("Finding");
    }
}