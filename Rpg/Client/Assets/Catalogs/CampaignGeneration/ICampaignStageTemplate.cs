using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal interface ICampaignStageTemplate
{
    public ICampaignStageItem Create();
}