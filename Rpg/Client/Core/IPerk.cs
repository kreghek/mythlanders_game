using System;
using System.Collections.Generic;

using Core.Combats;
using Core.Dices;

namespace Client.Core;

internal interface IPerk
{
    void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
    {
    }

    IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers()
    {
        return Array.Empty<(UnitStatType, IUnitStatModifier)>();
    }

    bool HandleEvasion(IDice dice)
    {
        return false;
    }

    int ModifyDamage(int sourceValue, IDice dice)
    {
        return sourceValue;
    }

    int ModifyHeal(int sourceValue, IDice dice)
    {
        return sourceValue;
    }
}