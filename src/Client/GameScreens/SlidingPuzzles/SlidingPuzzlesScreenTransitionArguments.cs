using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.SlidingPuzzles;

internal sealed class SlidingPuzzlesScreenTransitionArguments : IScreenTransitionArguments
{
    public SlidingPuzzlesScreenTransitionArguments(HeroCampaign campaign)
    {
        Campaign = campaign;
    }

    public HeroCampaign Campaign { get; }
}