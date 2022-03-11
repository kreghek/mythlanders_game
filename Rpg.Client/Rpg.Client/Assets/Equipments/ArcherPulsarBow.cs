using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class ArcherPulsarBow : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;
        public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.EnergyShot, SkillSid.RapidShot };

        protected override float MultiplicatorByLevel => 0.25f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}