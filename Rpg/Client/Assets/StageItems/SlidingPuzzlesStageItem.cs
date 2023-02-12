using Client.GameScreens.SlidingPuzzles;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class SlidingPuzzlesMinigameStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.SlidingPuzzlesMinigame,
            new SlidingPuzzlesMinigameScreenTransitionArguments(currentCampaign));
    }
}
