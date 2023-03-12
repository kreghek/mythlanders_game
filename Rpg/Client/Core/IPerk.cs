using System.Collections.Generic;

using Core.Dices;

using Core.Combats;
using Rpg.Client.Core.Skills;

namespace Client.Core;

internal interface IPerk : ICombatConditionEffectSource
{
    void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
    {
    }

    IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers()
    {
        return System.Array.Empty<(UnitStatType, IUnitStatModifier)>();
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