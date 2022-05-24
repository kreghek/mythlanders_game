using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Priest
{
    internal sealed class ScarabeusKingLeg : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ScarabeusKingLeg;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Priest;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}