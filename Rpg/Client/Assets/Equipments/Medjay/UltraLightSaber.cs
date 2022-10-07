using Client;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Medjay
{
    internal sealed class UltraLightSaber : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Medjay;
        public override EquipmentSid Sid => EquipmentSid.TribalEquipment;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.DieBySword, SkillSid.PoisonedSpear };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}