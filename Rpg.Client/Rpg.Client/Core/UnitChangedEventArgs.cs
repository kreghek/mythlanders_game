using System;

namespace Rpg.Client.Core
{
    internal class UnitChangedEventArgs : EventArgs
    {
        public ICombatUnit? NewUnit { get; init; }
        public ICombatUnit? OldUnit { get; init; }
    }
}