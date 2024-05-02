using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens;

internal abstract record CampaignScreenTransitionArgumentsBase(HeroCampaign Campaign) : IScreenTransitionArguments
{
    public bool IsGoalStage { get; init; }
}