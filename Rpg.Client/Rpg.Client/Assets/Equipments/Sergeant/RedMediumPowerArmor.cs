using System;
using System.Collections.Generic;

using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
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

        public IReadOnlyList<EffectRule> CreateCombatBeginingEffects(int equipmentLevel)
        {
            return new[] {
                new EffectRule
                {
                    Direction = SkillDirection.AllFriendly,
                    EffectCreator = new EffectCreator(u =>
                    {
                        var effect = new IncreaseDamagePercentEffect(u, effectLifetime: new UnitBoundEffectLifetime(u.Unit), multiplier: (equipmentLevel + 1) * 0.5f)
                        {
                            Visualization = EffectVisualizations.PowerUp
                        };
                        return effect;
                    })
                }
            };
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 2
        };
    }
}