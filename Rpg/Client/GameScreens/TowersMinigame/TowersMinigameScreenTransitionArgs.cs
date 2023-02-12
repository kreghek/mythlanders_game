using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.TowersMinigame;

internal sealed class TowersMinigameScreenTransitionArguments: IScreenTransitionArguments
{
    public TowersMinigameScreenTransitionArguments(HeroCampaign campaign) {
        Campaign = campaign;
    }

    public HeroCampaign Campaign { get; }
}