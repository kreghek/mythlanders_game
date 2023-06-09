using Client.Core.Campaigns;
using Client.GameScreens.SlidingPuzzles;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class SlidingPuzzlesStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.SlidingPuzzles,
            new SlidingPuzzlesScreenTransitionArguments(currentCampaign));
    }
}