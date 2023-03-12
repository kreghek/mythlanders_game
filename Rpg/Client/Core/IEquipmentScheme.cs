using System;
using System.Collections.Generic;

using Core.Combats;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IEquipmentScheme : ICombatConditionEffectSource
    {
        IEquipmentSchemeMetadata? Metadata { get; }
        public EquipmentItemType RequiredResourceToLevelUp { get; }

        EquipmentSid Sid { get; }

        float GetDamageMultiplierBonus(SkillSid skillSid, int level)
        {
            return 0;
        }

        string GetDescription();

        float GetHealMultiplierBonus(SkillSid skillSid, int level)
        {
            return 0;
        }

        IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers(int equipmentLevel)
        {
            return Array.Empty<(UnitStatType, IUnitStatModifier)>();
        }
    }
}