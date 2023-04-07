using Client.Core.Campaigns;

namespace Client.GameScreens.Crisis;

internal sealed record CrisisScreenTransitionArguments
    (HeroCampaign Campaign) : CampaignScreenTransitionArgumentsBase(Campaign);