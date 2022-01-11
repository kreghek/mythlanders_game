using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class WarriorSword: SimpleAttackEquipmentBase
    {
        public override EquipmentSid Sid => EquipmentSid.ArcherPulsarBow;
        
        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.SwordSlash, SkillSid.WideSwordSlash };

        protected override float MultiplicatorByLevel => 0.5f;

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
    }
}