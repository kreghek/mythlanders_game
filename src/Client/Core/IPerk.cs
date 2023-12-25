using System;
using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Dices;

namespace Client.Core;

internal interface IPerk
{
    void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
    {
    }

    IReadOnlyCollection<(ICombatantStatType, IStatModifier)> GetStatModifiers()
    {
        return Array.Empty<(ICombatantStatType, IStatModifier)>();
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