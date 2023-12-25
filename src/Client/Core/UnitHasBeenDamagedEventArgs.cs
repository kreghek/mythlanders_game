using System;

namespace Client.Core;

internal sealed class UnitHasBeenDamagedEventArgs : EventArgs
{
    public DamageResult? Result { get; init; }
}