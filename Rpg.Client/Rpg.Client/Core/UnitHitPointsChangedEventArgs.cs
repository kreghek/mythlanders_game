using System;

namespace Rpg.Client.Core
{
    internal class UnitHitPointsChangedEventArgs : EventArgs
    {
        public int Amount { get; init; }
        public int SourceAmount { get; init; }
        public CombatUnit? CombatUnit { get; init; }

        public HitPointsChangeDirection Direction { get; init; }
    }
}