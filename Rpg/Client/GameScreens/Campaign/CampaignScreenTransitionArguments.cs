using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Campaign;

internal sealed record CampaignScreenTransitionArguments
    (HeroCampaign Campaign) : CampaignScreenTransitionArgumentsBase(Campaign), IScreenTransitionArguments;