using System;
using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Sergeant
{
    internal sealed class RedMediumPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.RedMediumPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<EffectRule> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        {
            return new[]
            {
                SkillRuleFactory.CreatePowerUpFixed(_bonusesByLevel[context.EquipmentLevel], SkillDirection.AllFriendly)
            };
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 2
        };

        private int[] _bonusesByLevel = new[] {
            1,
            2,
            3,
            5,
            8
        };
    }
}