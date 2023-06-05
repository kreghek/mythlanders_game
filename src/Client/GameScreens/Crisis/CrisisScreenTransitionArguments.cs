using Client.Core.Campaigns;

using Core.Crises;

namespace Client.GameScreens.Crisis;

internal sealed record CrisisScreenTransitionArguments
    (HeroCampaign Campaign, EventType EventType) : CampaignScreenTransitionArgumentsBase(Campaign);