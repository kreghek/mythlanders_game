using System;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record ProgressionRewardStat
    {
        public int Amount { get; init; }
        public int StartValue { get; init; }
        public Func<int> ValueToLevelupSelector { get; init; }
    }
}