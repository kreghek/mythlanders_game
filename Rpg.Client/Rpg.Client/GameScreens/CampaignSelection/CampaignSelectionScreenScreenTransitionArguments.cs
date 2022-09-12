using System.Collections.Generic;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.CampaignSelection
{
    internal sealed class CampaignSelectionScreenScreenTransitionArguments : IScreenTransitionArguments
    {
        public IReadOnlyList<HeroCampaign> Campaigns { get; set; }
    }
}
