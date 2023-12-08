using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.CampaignRewards;

namespace Client.Core.Campaigns;

internal sealed record HeroCampaignLaunch(HeroCampaignLocation Location, IReadOnlyCollection<HeroState> Heroes, IReadOnlyCollection<ICampaignReward> Penalties) {
    public IReadOnlyCollection<ICampaignReward> Rewards => Location.Stages.GetAllNodes().Select(x => x.Payload)
            .OfType<IRewardCampaignStageItem>().First().GetEstimateRewards(Location);
};
