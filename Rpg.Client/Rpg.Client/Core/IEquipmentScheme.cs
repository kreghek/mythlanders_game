using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IEquipmentScheme: IEffectSource
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

        IReadOnlyList<EffectRule> CreateCombatBeginingEffects(int equipmentLevel)
        {
            return Array.Empty<EffectRule>();
        }

        float GetHitPointsMultiplier(int level)
        {
            return 1f;
        }
    }
}