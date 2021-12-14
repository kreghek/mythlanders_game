using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record UnitRewards
    {
        public RewardStat? Equipment { get; set; }
        public Unit Unit { get; init; }
        public RewardStat? Xp { get; set; }
    }
}