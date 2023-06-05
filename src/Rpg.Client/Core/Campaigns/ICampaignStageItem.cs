using Rpg.Client.ScreenManagement;

namespace Rpg.Client.Core.Campaigns
{
    internal interface ICampaignStageItem
    {
        void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager);
    }
}