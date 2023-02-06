using Client.GameScreens.CommandCenter;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems
{
    internal class RewardStageItem : ICampaignStageItem
    {
        private readonly ICampaignGenerator _campaignGenerator;

        public RewardStageItem(ICampaignGenerator campaignGenerator)
        {
            _campaignGenerator = campaignGenerator;
        }

        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
        {
            var campaigns = _campaignGenerator.CreateSet();
            screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignSelection,
                new CommandCenterScreenTransitionArguments
                {
                    AvailableCampaigns = campaigns
                });
        }
    }
}