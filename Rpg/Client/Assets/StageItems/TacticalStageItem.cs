using Client.GameScreens.Tactical;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems
{
    internal sealed class TacticalStageItem : ICampaignStageItem
    {
        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
        {
            screenManager.ExecuteTransition(currentScreen, ScreenTransition.Tactical, new TacticalScreenTransitionArguments(currentCampaign));
        }
    }
}