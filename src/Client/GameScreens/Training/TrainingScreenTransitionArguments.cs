using System.Collections.Generic;

using Client.Core;
using Client.Core.Campaigns;
using Client.ScreenManagement;

namespace Client.GameScreens.Training;

internal sealed class TrainingScreenTransitionArguments : IScreenTransitionArguments
{
    public TrainingScreenTransitionArguments(IReadOnlyList<HeroState> availableHeroes, HeroCampaign campaign)
    {
        AvailableHeroes = availableHeroes;
        Campaign = campaign;
    }

    public IReadOnlyList<HeroState> AvailableHeroes { get; }

    internal HeroCampaign Campaign { get; }
}