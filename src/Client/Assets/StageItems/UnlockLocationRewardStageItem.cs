using System.Collections.Generic;
using System.Linq;

using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.CampaignEffects;
using Client.Core.CampaignRewards;
using Client.Core.Campaigns;
using Client.GameScreens.CampaignReward;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class UnlockLocationRewardStageItem : IRewardCampaignStageItem
{
    private readonly GlobeProvider _globeProvider;

    private readonly IJobProgressResolver _jobProgressResolver;
    private readonly ILocationSid _scoutedLocation;

    public UnlockLocationRewardStageItem(GlobeProvider globeProvider,
        IJobProgressResolver jobProgressResolver,
        ILocationSid scoutedLocation)
    {
        _globeProvider = globeProvider;
        _scoutedLocation = scoutedLocation;
        _globeProvider = globeProvider;
        _jobProgressResolver = jobProgressResolver;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var completeCampaignProgress = new CampaignCompleteJobProgress();
        var currentJobs = _globeProvider.Globe.ActiveStoryPoints.ToArray();

        foreach (var job in currentJobs)
        {
            _jobProgressResolver.ApplyProgress(completeCampaignProgress, job);
        }

        _globeProvider.Globe.Player.AddLocation(_scoutedLocation);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.CampaignReward,
            new CampaignRewardScreenTransitionArguments(currentCampaign, GetEstimateRewards(currentCampaign.Location)));
    }

    public IReadOnlyCollection<ICampaignEffect> GetEstimateRewards(HeroCampaignLocation heroCampaign)
    {
        return new[] { new LocationCampaignEffect(_scoutedLocation) };
    }
}