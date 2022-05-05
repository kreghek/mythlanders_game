using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class FlameThrower : SimpleBonusEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Engineer;
        public override EquipmentSid Sid => EquipmentSid.FlameThrower;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.FlameThrowing, SkillSid.PipeBludgeon };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}