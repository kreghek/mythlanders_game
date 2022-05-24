using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Equipments.Swordsman
{
    internal sealed class Mk2MediumPowerArmor : IEquipmentScheme
    {
        public EquipmentSid Sid => EquipmentSid.Mk2MediumPowerArmor;

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

        public IEquipmentSchemeMetadata? Metadata => new EquipmentSchemeMetadata
        {
            IconOneBasedIndex = 2
        };

        public IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers(int equipmentLevel)
        {
            return new (UnitStatType, IUnitStatModifier)[]
            {
                new(UnitStatType.HitPoints, new StatModifier(equipmentLevel * 0.2f))
            };
        }
    }
}