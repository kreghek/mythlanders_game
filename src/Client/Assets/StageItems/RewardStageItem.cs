using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class RewardStageItem : IRewardCampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign));
    }
}