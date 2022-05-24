using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Liberator
{
    internal sealed class HiddenExoskeleton : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.HiddenExoskeleton;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Liberator;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}