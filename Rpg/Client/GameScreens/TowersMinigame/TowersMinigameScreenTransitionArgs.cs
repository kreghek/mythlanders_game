using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.TowersMinigame;

internal sealed class TowersMinigameScreenTransitionArgs: IScreenTransitionArguments
{
    public TowersMinigameScreenTransitionArgs(HeroCampaign campaign) {
        Campaign = campaign;
    }

    public HeroCampaign Campaign { get; }
}