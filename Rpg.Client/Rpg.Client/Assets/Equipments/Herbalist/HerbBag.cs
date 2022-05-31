using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Herbalist
{
    internal sealed class HerbBag : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 7
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;
        public override EquipmentSid Sid => EquipmentSid.HerbBag;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.HealingSalve, SkillSid.ToxicGas };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            throw new InvalidOperationException();
        }
    }
}