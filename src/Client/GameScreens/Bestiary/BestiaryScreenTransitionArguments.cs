using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Bestiary;

internal sealed record BestiaryScreenTransitionArguments
    (IReadOnlyList<HeroCampaignLaunch> AvailableLaunches) : IScreenTransitionArguments;