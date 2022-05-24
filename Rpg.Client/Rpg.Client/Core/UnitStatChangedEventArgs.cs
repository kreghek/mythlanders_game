using System;

namespace Rpg.Client.Core
{
    internal class UnitStatChangedEventArgs : EventArgs
    {
        public int Amount { get; init; }
        public CombatUnit? CombatUnit { get; init; }

        public HitPointsChangeDirection Direction { get; init; }
        public int SourceAmount { get; init; }
    }
}