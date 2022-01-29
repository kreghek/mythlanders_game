using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments
{
    internal sealed class VoiceModulator : SimpleAttackEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Liberator;
        public override EquipmentSid Sid => EquipmentSid.VoiceModulator;

        protected override SkillSid[] AffectedAttackingSkills =>
            new[] { SkillSid.Liberation };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }
    }
}