using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Archer
{
    internal sealed class ArcherPulsarBow : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
        public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.EnergyShot, SkillSid.RapidShot };

        protected override float MultiplicatorByLevel => 0.25f;

        public override string GetDescription()
        {
            throw new InvalidOperationException();
        }
    }
}