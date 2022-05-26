using System;
using System.Collections.Generic;

using Rpg.Client.Assets.SkillEffects;
using Rpg.Client.Core;
using Rpg.Client.Core.SkillEffects;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Archer
{
    internal sealed class SilverWindNecklace : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.SilverWindNecklace;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 6
        };

        public IReadOnlyList<EffectRule> CreateCombatBeginningEffects(IEquipmentEffectContext context)
        {
            return new[]
            {
                new EffectRule
                {
                    Direction = SkillDirection.OtherFriendly,
                    EffectCreator = new EffectCreator(u =>
                    {
                        var lifetime = new UnitBoundEffectLifetime(u);
                        return new ShieldPointModifyEffect(u, lifetime, 0.2f * context.EquipmentLevel);
                    })
                }
            };
        }
    }
}