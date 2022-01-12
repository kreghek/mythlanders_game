using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class WarriorGreatSword : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
        public override EquipmentSid Sid => EquipmentSid.WarriorGreatSword;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.SwordSlash, SkillSid.WideSwordSlash };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}