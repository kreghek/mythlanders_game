using Client.ScreenManagement;

namespace Client.Core.Campaigns;

internal interface ICampaignStageItem
{
    void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign);
    bool IsGoalStage { get; }
}