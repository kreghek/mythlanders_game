using Client.Core.Campaigns;
using Client.GameScreens.Match3;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class Match3MinigameStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Match3Minigame,
            new Match3MiniGameScreenTransitionArguments(currentCampaign));
    }

    public bool IsReward { get; }
}