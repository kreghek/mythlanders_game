using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Herbalist
{
    internal sealed class WomanShort : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.WomanShort;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Herbalist;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 8
        };
    }
}