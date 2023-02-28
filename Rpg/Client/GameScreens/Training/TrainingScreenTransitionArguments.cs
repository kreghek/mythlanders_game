using System.Collections.Generic;

using Client.Core.Campaigns;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Training
{
    internal sealed class TrainingScreenTransitionArguments : IScreenTransitionArguments
    {
        public TrainingScreenTransitionArguments(IReadOnlyList<Unit> availableUnits, HeroCampaign campaign)
        {
            AvailableUnits = availableUnits;
            Campaign = campaign;
        }

        public IReadOnlyList<Unit> AvailableUnits { get; init; }

        internal HeroCampaign Campaign { get; }
    }
}