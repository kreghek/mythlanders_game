using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Legionnaire
{
    internal sealed class EmberGladius : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Legionnaire;
        public override EquipmentSid Sid => EquipmentSid.EmberGladius;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.SwordSwing, SkillSid.AresWarBringerThreads };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Aspid;
        }
    }
}