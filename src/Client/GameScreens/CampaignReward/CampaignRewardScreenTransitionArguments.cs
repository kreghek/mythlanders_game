using System.Collections.Generic;

using Client.Core.Campaigns;

namespace Client.GameScreens.CampaignReward;

internal sealed record CampaignRewardScreenTransitionArguments
    (HeroCampaign Campaign, IReadOnlyCollection<ICampaignReward> CampaignRewards) :
        CampaignScreenTransitionArgumentsBase(
            Campaign);