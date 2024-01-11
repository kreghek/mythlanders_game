using Client.Core.Campaigns;

namespace Client.GameScreens.CampaignReward;

internal sealed record CampaignRewardScreenTransitionArguments
    (HeroCampaign Campaign) :
        CampaignScreenTransitionArgumentsBase(
            Campaign);