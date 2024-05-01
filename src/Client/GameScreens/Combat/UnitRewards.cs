using Client.GameScreens.Common.Result;

namespace Client.GameScreens.Combat;

internal sealed record CombatRewardsItem
{
    public ResourceReward? Xp { get; set; }
}