using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Monk
{
    internal sealed class SymbolOfGod : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.SymbolOfGod;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Monk;

        public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}