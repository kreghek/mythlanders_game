using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Spearman
{
    internal sealed class JuggernautHeavyPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.JuggernautHeavyPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        // public IReadOnlyList<EffectRule> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        // {
        //     if (context.IsInTankingSlot)
        //     {
        //         return new[]
        //         {
        //             SkillRuleFactory.CreateProtection(context.EquipmentLevel, direction: SkillDirection.AllFriendly,
        //                 equipmentMultiplier: 0.5f)
        //         };
        //     }
        //
        //     return Array.Empty<EffectRule>();
        // }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Spearman;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}