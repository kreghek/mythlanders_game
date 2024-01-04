using Client.Core.Campaigns;

namespace Client.GameScreens.TowersMinigame;

internal sealed record TowersMiniGameScreenTransitionArguments
    (HeroCampaign Campaign) : CampaignScreenTransitionArgumentsBase(Campaign);