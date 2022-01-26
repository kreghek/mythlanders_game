using Rpg.Client.Core.Skills;

namespace Rpg.Client.Core.Equipments
{
    internal sealed class EliteGuardsmanSpear : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Spearman;
        public override EquipmentSid Sid => EquipmentSid.EliteGuardsmanSpear;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.PenetrationStrike, SkillSid.StonePath };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}