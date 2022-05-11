using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;
using Rpg.Client.Core.Skills;

namespace Rpg.Client.Assets.Equipments.Spearman
{
    internal sealed class EliteGuardsmanSpear : SimpleBonusEquipmentBase
    {
        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Spearman;
        public override EquipmentSid Sid => EquipmentSid.EliteGuardsmanSpear;

        protected override SkillSid[] AffectedSkills =>
            new[] { SkillSid.PenetrationStrike, SkillSid.StonePath };

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            return GameObjectResources.Hawk;
        }

        public override IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}