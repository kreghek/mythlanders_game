using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class TribalEquipment : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Scorpion;
        public override EquipmentSid Sid => EquipmentSid.TribalEquipment;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.SwordSlash, SkillSid.PoisonedSpear };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}