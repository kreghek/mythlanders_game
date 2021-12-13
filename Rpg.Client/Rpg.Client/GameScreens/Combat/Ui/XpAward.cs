using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record UnitRewards
    {
        public Unit Unit { get; init; }
        public RewardStat? Xp { get; set; }
        public RewardStat? Equipment { get; set; }
    }

    internal sealed record RewardStat
    {
        public int StartValue { get; init; }
        public int Amount { get; init; }
        public Func<int> ValueToLevelupSelector { get; init; }
    }
}