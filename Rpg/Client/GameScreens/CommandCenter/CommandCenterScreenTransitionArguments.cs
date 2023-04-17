using System.Collections.Generic;

using Client.Core.Campaigns;

using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.CommandCenter
{
    internal sealed class CommandCenterScreenTransitionArguments : IScreenTransitionArguments
    {
        public IReadOnlyList<HeroCampaign> AvailableCampaigns { get; set; }
    }
}