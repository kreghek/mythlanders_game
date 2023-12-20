using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Training;

internal sealed class TrainingScreenTransitionArguments : IScreenTransitionArguments
{
    public TrainingScreenTransitionArguments(IReadOnlyList<HeroState> availableUnits, HeroCampaign campaign)
    {
        AvailableUnits = availableUnits;
        Campaign = campaign;
    }

    public IReadOnlyList<HeroState> AvailableUnits { get; init; }

    internal HeroCampaign Campaign { get; }
}