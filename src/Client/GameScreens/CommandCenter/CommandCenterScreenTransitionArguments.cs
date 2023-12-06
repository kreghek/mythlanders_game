using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.CommandCenter;

internal sealed class CommandCenterScreenTransitionArguments : IScreenTransitionArguments
{
    public IReadOnlyList<HeroCampaignSource> AvailableCampaigns { get; }

    public CommandCenterScreenTransitionArguments(IReadOnlyList<HeroCampaignSource> availableCampaigns)
    {
        AvailableCampaigns = availableCampaigns;
    }
}