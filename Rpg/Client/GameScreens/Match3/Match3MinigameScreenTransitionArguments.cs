using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.GameScreens.Match3;

internal sealed class Match3MinigameScreenTransitionArguments : IScreenTransitionArguments
{
    public Match3MinigameScreenTransitionArguments(HeroCampaign campaign)
    {
        Campaign = campaign;
    }

    public HeroCampaign Campaign { get; }
}
