using System.Collections.Generic;

using Client.Core.Campaigns;
using Client.Core.Heroes;
using Client.ScreenManagement;

namespace Client.GameScreens.Training;

internal sealed class TrainingScreenTransitionArguments : IScreenTransitionArguments
{
    public TrainingScreenTransitionArguments(IReadOnlyList<Hero> availableUnits, HeroCampaign campaign)
    {
        AvailableUnits = availableUnits;
        Campaign = campaign;
    }

    public IReadOnlyList<Hero> AvailableUnits { get; init; }

    internal HeroCampaign Campaign { get; }
}