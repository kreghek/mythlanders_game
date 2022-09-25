using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Zoologist
{
    internal sealed class ScientistRobe : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ScientistRobe;

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