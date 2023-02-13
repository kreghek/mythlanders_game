using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class ChallengeCampaignStageTemplate : ICampaignStageTemplate
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStage("Challenge");
    }
}
