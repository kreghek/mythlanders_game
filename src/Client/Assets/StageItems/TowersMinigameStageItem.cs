using Client.Core.Campaigns;
using Client.GameScreens.TowersMinigame;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class TowersMinigameStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.TowersMinigame,
            new TowersMiniGameScreenTransitionArguments(currentCampaign));
    }
}