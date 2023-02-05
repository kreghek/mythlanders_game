using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.SlidingPuzzles
{
    internal sealed class SlidingPuzzlesScreenTransitionArguments : IScreenTransitionArguments
    {
        public HeroCampaign Campaign { get; }

        public SlidingPuzzlesScreenTransitionArguments(HeroCampaign campaign)
        {
            Campaign = campaign;
        }
    }
}