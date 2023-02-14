using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class ChallengeCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStageItem("Challenge");
    }
}