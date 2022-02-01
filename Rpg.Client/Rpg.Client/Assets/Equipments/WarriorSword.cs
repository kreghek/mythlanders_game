using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class CombatSword : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
        public override EquipmentSid Sid => EquipmentSid.CombatSword;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.SwordSlash, SkillSid.WideSwordSlash };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }

    internal sealed class EmberGladius : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Legionnaire;
        public override EquipmentSid Sid => EquipmentSid.EmberGladius;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.SwordSwing, SkillSid.AresWarBringerThreadsSkill };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}