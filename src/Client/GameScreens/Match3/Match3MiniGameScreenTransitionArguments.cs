using Client.Core.Campaigns;

namespace Client.GameScreens.Match3;

internal sealed record Match3MiniGameScreenTransitionArguments
    (HeroCampaign Campaign) : CampaignScreenTransitionArgumentsBase(Campaign);