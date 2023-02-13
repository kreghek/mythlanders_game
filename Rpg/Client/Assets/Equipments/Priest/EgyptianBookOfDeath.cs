using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Priest
{
    internal sealed class EgyptianBookOfDeath : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Priest;
        public override EquipmentSid Sid => EquipmentSid.EgyptianBookOfDeath;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.DarkLighting, SkillSid.UnlimitedSin };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            throw new InvalidOperationException();
        }
    }
}