using System.Collections.Generic;

using Client.Core.CampaignEffects;
using Client.Core.Campaigns;

namespace Client.GameScreens.CampaignReward;

internal sealed record CampaignRewardScreenTransitionArguments
    (HeroCampaign Campaign, IReadOnlyCollection<ICampaignEffect> CampaignRewards) :
        CampaignScreenTransitionArgumentsBase(
            Campaign);