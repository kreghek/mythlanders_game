using Client.Core.Campaigns;

namespace Client.GameScreens.Rest;

internal sealed record RestScreenTransitionArguments
    (HeroCampaign Campaign) : CampaignScreenTransitionArgumentsBase(Campaign);