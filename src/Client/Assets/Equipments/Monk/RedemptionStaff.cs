using System;

using Rpg.Client.Core;
using Rpg.Client.Core.Equipments;

namespace Rpg.Client.Assets.Equipments.Monk
{
    internal sealed class RedemptionStaff : SimpleBonusEquipmentBase
    {
        public override IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };

        public override EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;
        public override EquipmentSid Sid => EquipmentSid.HerbBag;

        protected override float MultiplicatorByLevel => 0.5f;

        public override string GetDescription()
        {
            throw new InvalidOperationException();
        }
    }
}