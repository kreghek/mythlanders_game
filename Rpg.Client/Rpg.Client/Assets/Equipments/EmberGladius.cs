using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class EmberGladius : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Legionnaire;
        public override EquipmentSid Sid => EquipmentSid.EmberGladius;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.SwordSwing, SkillSid.AresWarBringerThreads };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}