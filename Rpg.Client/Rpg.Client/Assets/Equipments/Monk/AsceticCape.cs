using System;
using System.Collections.Generic;

using Rpg.Client.Assets.SkillEffects;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Monk
{
    internal sealed class AsceticCape : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.AsceticRobe;

        public IReadOnlyList<EffectRule> CreateCombatHitpointChangeEffects(IEquipmentEffectContext context)
        {
            return new[] {
                new EffectRule{
                    Direction = SkillDirection.Self,
                    EffectCreator = new EffectCreator(u =>{
                        return new PeriodicHealEffect(u, new HitpointThresholdEffectLifetime(u.Unit, 0.5f));
                    })
                }
            };
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}