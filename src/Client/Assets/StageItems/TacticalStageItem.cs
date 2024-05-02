using Client.Core.Campaigns;
using Client.GameScreens.Tactical;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class TacticalStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Tactical,
            new TacticalScreenTransitionArguments(currentCampaign));
    }

    public bool IsReward { get; }
}