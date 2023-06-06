using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Tactical;

internal sealed class TacticalScreenTransitionArguments : IScreenTransitionArguments
{
    public TacticalScreenTransitionArguments(HeroCampaign heroCampaign)
    {
        HeroCampaign = heroCampaign;
    }

    public HeroCampaign HeroCampaign { get; }
}