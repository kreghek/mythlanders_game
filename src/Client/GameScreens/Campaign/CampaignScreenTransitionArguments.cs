using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Campaign;

internal sealed record CampaignScreenTransitionArguments
    (HeroCampaign Campaign) : CampaignScreenTransitionArgumentsBase(Campaign), IScreenTransitionArguments;