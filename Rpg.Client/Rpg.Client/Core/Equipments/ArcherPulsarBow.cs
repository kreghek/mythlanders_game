using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class ArcherPulsarBow : SimpleAttackEquipmentBase
    {
        public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.EnergyShot, SkillSid.RapidEnergyShot };

        protected override float MultiplicatorByLevel => 0.5f;

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
    }
}