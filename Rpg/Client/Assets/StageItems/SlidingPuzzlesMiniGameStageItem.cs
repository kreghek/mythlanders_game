using Client.Core.Campaigns;
using Client.GameScreens.SlidingPuzzles;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class SlidingPuzzlesMiniGameStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.SlidingPuzzles,
            new SlidingPuzzlesMinigameScreenTransitionArguments(currentCampaign));
    }
}