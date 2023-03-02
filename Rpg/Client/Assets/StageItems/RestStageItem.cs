using Client.Core.Campaigns;
using Client.GameScreens.Rest;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class RestStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Rest,
            new RestScreenTransitionArguments(currentCampaign));
    }
}