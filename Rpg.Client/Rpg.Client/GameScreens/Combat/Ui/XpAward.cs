using System;

using Rpg.Client.Core;

namespace Rpg.Client.GameScreens.Combat.Ui
{
    internal sealed record XpAward
    {
        public bool IsLevelUp => StartXp + XpAmount >= XpToLevelupSelector();
        public int StartXp { get; init; }
        public Unit Unit { get; init; }
        public int XpAmount { get; init; }
        public Func<int> XpToLevelupSelector { get; init; }
    }
}