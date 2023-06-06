using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Training;

internal sealed class TrainingScreenTransitionArguments : IScreenTransitionArguments
{
    public TrainingScreenTransitionArguments(IReadOnlyList<Core.Heroes.Hero> availableUnits, HeroCampaign campaign)
    {
        AvailableUnits = availableUnits;
        Campaign = campaign;
    }

    public IReadOnlyList<Core.Heroes.Hero> AvailableUnits { get; init; }

    internal HeroCampaign Campaign { get; }
}