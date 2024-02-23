using System;

using Core.Props;

namespace Client.Core;

internal sealed class Equipment: IProp
{
    public Equipment(IEquipmentScheme scheme)
    {
        Scheme = scheme;
    }

    public int Level { get; private set; }

    public int RequiredResourceAmountToLevelUp => (int)Math.Pow(2, Level + 1);

    public IEquipmentScheme Scheme { get; }
    IPropScheme IProp.Scheme { get; }

    public void LevelUp()
    {
        Level++;

        GainLevelUp?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? GainLevelUp;
}
