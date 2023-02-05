using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Training
{
    internal sealed class TrainingScreenTransitionArguments : IScreenTransitionArguments
    {
        private readonly HeroCampaign _campaign;

        public TrainingScreenTransitionArguments(IReadOnlyList<Unit> availableUnits, HeroCampaign campaign)
        {
            AvailableUnits = availableUnits;
            _campaign = campaign;
        }

        public IReadOnlyList<Unit> AvailableUnits { get; init; }

        internal HeroCampaign Campaign => _campaign;
    }
}