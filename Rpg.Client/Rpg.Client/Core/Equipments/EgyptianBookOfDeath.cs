using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class EgyptianBookOfDeath : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Priest;
        public override EquipmentSid Sid => EquipmentSid.EgyptianBookOfDeath;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.DarkLighting, SkillSid.MummificationTouch };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}