using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Archer
{
    internal sealed class SilverWindNecklace : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.SilverWindNecklace;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Archer;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 6
        };

        public IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers(int equipmentLevel)
        {
            return new (UnitStatType, IUnitStatModifier)[] {
                new (UnitStatType.ShieldPoints, new StatModifier(equipmentLevel * 0.2f))
            };
        }
    }
}