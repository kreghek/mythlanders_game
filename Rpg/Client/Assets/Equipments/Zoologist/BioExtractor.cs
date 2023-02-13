using Client;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Zoologist
{
    internal sealed class BioExtractor : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;
        public override EquipmentSid Sid => EquipmentSid.CombatSword;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.DieBySword, SkillSid.WideSwordSlash };

        protected override float MultiplicatorByLevel => 0.25f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}