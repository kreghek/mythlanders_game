using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class FindingEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public ICampaignStageItem Create()
    {
        return new NotImplemenetedStageItem("Finding");
    }
}