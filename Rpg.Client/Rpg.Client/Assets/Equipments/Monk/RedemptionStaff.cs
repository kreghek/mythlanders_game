using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Monk
{
    internal sealed class RedemptionStaff : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;
        public override EquipmentSid Sid => EquipmentSid.HerbBag;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.StaffHit, SkillSid.MasterStaffHit };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            throw new System.InvalidOperationException();
        }
    }
}