using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.CampaignRewards;
using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class UnlockHeroRewardStageItem : IRewardCampaignStageItem
{
    private readonly GlobeProvider _globeProvider;
    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly UnitName _jointedHeroName;

    public UnlockHeroRewardStageItem(GlobeProvider globeProvider,
        IJobProgressResolver jobProgressResolver,
        UnitName jointedHeroName)
    {
        _globeProvider = globeProvider;
        _jobProgressResolver = jobProgressResolver;
        _jointedHeroName = jointedHeroName;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var completeCampaignProgress = new CampaignCompleteJobProgress();
        var currentJobs = _globeProvider.Globe.ActiveStoryPoints.ToArray();

        foreach (var job in currentJobs)
        {
            _jobProgressResolver.ApplyProgress(completeCampaignProgress, job);
        }

        _globeProvider.Globe.Player.AddHero(HeroState.Create(_jointedHeroName.ToString().ToLowerInvariant()));

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign, GetEstimateRewards(currentCampaign.Location)));
    }

    public IReadOnlyCollection<ICampaignReward> GetEstimateRewards(HeroCampaignLocation heroCampaign)
    {
        return new[] { new HeroCampaignReward(_jointedHeroName) };
    }
}