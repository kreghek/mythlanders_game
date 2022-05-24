using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Swordsman
{
    internal sealed class WoodenHandSculpture : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.WoodenHandSculpture;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 3
        };
    }
}