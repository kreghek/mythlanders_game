using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IPerk: ICombatConditionEffectSource
    {
        void ApplyToStats(ref float maxHitpoints, ref float armorBonus)
        {
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
}