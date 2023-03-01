using Client.Core.Campaigns;

using Rpg.Client.ScreenManagement;

namespace Client.GameScreens;

internal abstract record CampaignScreenTransitionArgumentsBase(HeroCampaign Campaign): IScreenTransitionArguments;