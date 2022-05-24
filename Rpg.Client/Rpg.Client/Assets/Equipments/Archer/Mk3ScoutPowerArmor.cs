using System;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Archer
{
    internal sealed class Mk3ScoutPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.ArcherMk3ScoutPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 5
        };
    }
}