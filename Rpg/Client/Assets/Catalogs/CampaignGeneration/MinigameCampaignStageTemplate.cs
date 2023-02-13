using Rpg.Client.Core.Campaigns;
using Client.Assets.StageItems;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class MinigameCampaignStageTemplate : ICampaignStageTemplate
{
    public ICampaignStageItem Create()
    {
        return new SlidingPuzzlesStageItem();
    }
}
