using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class EmptinessInTheHand : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Sage;
        public override EquipmentSid Sid => EquipmentSid.EmptinessInTheHand;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.NoViolencePleaseSkill, SkillSid.Reproach };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}