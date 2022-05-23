using System;
using System.Collections.Generic;

using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core
{
    internal interface IEquipmentScheme: IEffectSource, ICombatConditionEffectSource
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

        float GetHitPointsMultiplier(int level)
        {
            return 1f;
        }
    }

    internal interface IEquipmentEffectContext
    {
        int EquipmentLevel { get; }
        bool IsInTankingSlot { get; }
    }

    internal sealed class EquipmentEffectContext : IEquipmentEffectContext
    {
        public EquipmentEffectContext(CombatUnit combatUnit, Equipment equipment)
        {
            EquipmentLevel = equipment.Level;
            IsInTankingSlot = combatUnit.IsInTankLine;
        }

        public int EquipmentLevel { get; }
        public bool IsInTankingSlot { get; }
    }

    internal sealed class EmptyEffectContext : IEquipmentEffectContext
    {
        public EmptyEffectContext(CombatUnit combatUnit)
        {
            IsInTankingSlot = combatUnit.IsInTankLine;
        }
        
        public int EquipmentLevel { get; set; }
        public bool IsInTankingSlot { get; set; }
    }
}