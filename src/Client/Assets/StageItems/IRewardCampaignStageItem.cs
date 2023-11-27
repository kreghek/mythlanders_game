using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;

namespace Client.Assets.StageItems;

internal interface IRewardCampaignStageItem : ICampaignStageItem
{
    IReadOnlyCollection<ICampaignReward> GetEstimateRewards(HeroCampaign heroCampaign);
}