using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record CombatRewards
    {
        public RewardStat BiomeProgress { get; init; }
        public IReadOnlyCollection<UnitRewards> UnitRewards { get; init; }
    }

    internal sealed record UnitRewards
    {
        public RewardStat? Equipment { get; set; }
        public Unit Unit { get; init; }
        public RewardStat? Xp { get; set; }
    }

    internal sealed record RewardStat
    {
        public int Amount { get; init; }
        public int StartValue { get; init; }
        public Func<int> ValueToLevelupSelector { get; init; }
    }
}