using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class EgyptianBookOfDeath : SimpleBonusEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Priest;
        public override EquipmentSid Sid => EquipmentSid.EgyptianBookOfDeath;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.DarkLighting, SkillSid.MummificationTouch };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}