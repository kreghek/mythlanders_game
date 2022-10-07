using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Amazon
{
    internal sealed class HunterRifle : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Amazon;
        public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow2;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.PainfullWound, SkillSid.ShotOfHate };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            throw new InvalidOperationException();
        }
    }
}