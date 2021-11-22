using System;

namespace Rpg.Client.Core
{
    internal class CombatFinishEventArgs : EventArgs
    {
        public bool Victory { get; init; }
    }
}