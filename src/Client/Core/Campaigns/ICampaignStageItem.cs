using Client.ScreenManagement;

namespace Client.Core.Campaigns;

internal interface ICampaignStageItem
{
    void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign);
    
    /// <summary>
    /// Completion of this stage will complete campaign.
    /// </summary>
    bool IsGoalStage { get; }
}