using System;

namespace Rpg.Client.Core
{
    internal sealed class UnitHasBeenDamagedEventArgs : EventArgs
    {
        public DamageResult? Result { get; init; }
    }
}