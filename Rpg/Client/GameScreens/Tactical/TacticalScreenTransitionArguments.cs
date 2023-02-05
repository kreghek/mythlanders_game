using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Tactical
{
    internal sealed class TacticalScreenTransitionArguments : IScreenTransitionArguments
    {
        public HeroCampaign HeroCampaign { get; }

        public TacticalScreenTransitionArguments(HeroCampaign heroCampaign)
        {
            HeroCampaign = heroCampaign;
        }
    }
}