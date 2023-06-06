using Client.Core.Campaigns;
using Client.GameScreens.Crisis;
using Client.ScreenManagement;

using Core.Crises;

namespace Client.Assets.StageItems;

internal sealed class CrisisStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Crisis,
            new CrisisScreenTransitionArguments(currentCampaign, EventType.Crisis));
    }
}