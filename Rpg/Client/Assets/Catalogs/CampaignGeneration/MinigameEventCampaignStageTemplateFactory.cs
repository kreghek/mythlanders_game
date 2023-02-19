using Client.Assets.StageItems;

using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class MinigameEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public bool CanCreate()
    {
        return true;
    }

    public ICampaignStageItem Create()
    {
        return new SlidingPuzzlesStageItem();
    }
}