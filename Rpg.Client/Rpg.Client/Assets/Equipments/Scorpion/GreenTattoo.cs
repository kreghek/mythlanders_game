using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Scorpion
{
    internal sealed class GreenTattoo : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.GreenTattoo;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Scorpion;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 4
        };
    }
}