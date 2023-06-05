using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.GameScreens.Campaign
{
    internal sealed class CampaignScreenTransitionArguments : IScreenTransitionArguments
    {
        public HeroCampaign Campaign { get; set; }
    }
}
