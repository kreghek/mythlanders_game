using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Sergeant
{
    internal sealed class CompactSaber : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 1
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
        public override EquipmentSid Sid => EquipmentSid.CombatSword;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.SwordSlash, SkillSid.WideSwordSlash };

        protected override float MultiplicatorByLevel => 0.25f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}