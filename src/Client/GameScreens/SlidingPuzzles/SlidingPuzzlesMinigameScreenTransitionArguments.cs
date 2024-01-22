using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.SlidingPuzzles;

internal sealed class SlidingPuzzlesMinigameScreenTransitionArguments : IScreenTransitionArguments
{
    public SlidingPuzzlesMinigameScreenTransitionArguments(HeroCampaign campaign)
    {
        Campaign = campaign;
    }

    public HeroCampaign Campaign { get; }
}