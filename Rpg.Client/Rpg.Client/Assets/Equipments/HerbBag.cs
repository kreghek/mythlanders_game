using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class HerbBag : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;
        public override EquipmentSid Sid => EquipmentSid.HerbBag;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.HealingSalve, SkillSid.ToxicHerbs };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}