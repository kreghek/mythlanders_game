using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core.Campaigns;
using Client.GameScreens.CommandCenter;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems
{
    internal class RewardStageItem : ICampaignStageItem
    {
        private readonly ICampaignGenerator _campaignGenerator;
        private readonly GlobeProvider _globeProvider;
        private readonly IJobProgressResolver _jobProgressResolver;

        public RewardStageItem(ICampaignGenerator campaignGenerator, GlobeProvider globeProvider, IJobProgressResolver jobProgressResolver)
        {
            _campaignGenerator = campaignGenerator;
            _globeProvider = globeProvider;
            _jobProgressResolver = jobProgressResolver;
        }

        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
        {
            var completeCampaignProgress = new CampaignCompleteJobProgress();
            var currentJobs = _globeProvider.Globe.ActiveStoryPoints.ToArray();
            
            _jobProgressResolver.ApplyProgress(completeCampaignProgress, currentJobs);
                
            var campaigns = _campaignGenerator.CreateSet();
            screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignSelection,
                new CommandCenterScreenTransitionArguments
                {
                    AvailableCampaigns = campaigns
                });
        }
    }
}