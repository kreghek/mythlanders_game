using System;
using System.Collections.Generic;

using Core.Combats;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Monk
{
    internal sealed class AsceticCape : IEquipmentScheme
    {
        public IReadOnlyList<IEffect> CreateCombatHitPointsChangedEffects(IEquipmentEffectContext context)
        {
            //return new[]
            //{
            //    new EffectRule
            //    {
            //        Direction = SkillDirection.Self,
            //        EffectCreator = new EffectCreator(u =>
            //        {
            //            return new PeriodicHealEffect(u, new HitpointThresholdEffectLifetime((CombatUnit)u, 0.5f));
            //        })
            //    }
            //};

            return Array.Empty<IEffect>();
        }

        public EquipmentSid Sid => EquipmentSid.AsceticRobe;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}