using System;

namespace Rpg.Client.Core
{
    internal class UnitChangedEventArgs : EventArgs
    {
        public CombatUnit? NewUnit { get; init; }
        public CombatUnit? OldUnit { get; init; }
    }
}