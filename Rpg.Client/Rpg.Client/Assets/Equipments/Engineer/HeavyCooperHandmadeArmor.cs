using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Engineer
{
    internal sealed class HeavyCooperHandmadeArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.HeavyCooperHandmadeArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Engineer;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}