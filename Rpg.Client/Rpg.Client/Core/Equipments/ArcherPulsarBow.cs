using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class ArcherPulsarBow : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
        public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.EnergyShot, SkillSid.RapidShot };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}