using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Challenge;
using Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class ChallengeStageItem : ICampaignStageItem
{
    private readonly IReadOnlyCollection<IJob> _challengeJobs;

    public ChallengeStageItem(IReadOnlyCollection<IJob> challengeJobs)
    {
        _challengeJobs = challengeJobs;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Challenge,
            new ChallengeScreenTransitionArguments(currentCampaign, _challengeJobs));
    }

    public bool IsGoalStage { get; }
}