using System;
using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Spearman
{
    internal sealed class JuggernautHeavyPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.JuggernautHeavyPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<EffectRule> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        {
            if (context.IsInTankingSlot)
            {
                return new[] {
                    SkillRuleFactory.CreateProtection(context.EquipmentLevel, SkillDirection.AllFriendly)
                };
            }
            else
            {
                return Array.Empty<EffectRule>();
            }
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Spearman;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}