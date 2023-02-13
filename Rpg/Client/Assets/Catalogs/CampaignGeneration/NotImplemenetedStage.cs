using Client.GameScreens.NotImplementedStage;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class NotImplemenetedStage : ICampaignStageItem
{
    public NotImplemenetedStage(string stageSid)
    {
        StageSid = stageSid;
    }

    public string StageSid { get; }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.NotImplemented,
            new NotImplementedStageScreenTransitionArguments(currentCampaign));
    }
}
