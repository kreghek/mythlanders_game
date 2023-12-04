using System.Collections.Generic;

using Client.Core.CampaignRewards;
using Client.Core.Campaigns;

namespace Client.Assets.StageItems;

internal interface IRewardCampaignStageItem : ICampaignStageItem
{
    IReadOnlyCollection<ICampaignReward> GetEstimateRewards(HeroCampaign heroCampaign);
}