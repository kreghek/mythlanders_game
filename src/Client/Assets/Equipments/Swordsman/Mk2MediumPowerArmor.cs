using System;
using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Combats;

namespace Client.Assets.Equipments.Swordsman;

internal sealed class Mk2MediumPowerArmor : IEquipmentScheme
{
    public EquipmentSid Sid => EquipmentSid.Mk2MediumPowerArmor;

    public string GetDescription()
    {
        throw new NotImplementedException();
    }

    public EquipmentItemType RequiredResourceToLevelUp => EquipmentItemType.Warrior;

    public IEquipmentSchemeMetadata Metadata => new EquipmentSchemeMetadata
    {
        IconOneBasedIndex = 2
    };

    public IReadOnlyCollection<(ICombatantStatType, IUnitStatModifier)> GetStatModifiers(int equipmentLevel)
    {
        return new (ICombatantStatType, IUnitStatModifier)[]
        {
            new(CombatantStatTypes.HitPoints, new StatModifier((int)(equipmentLevel * 0.2f)))
        };
    }
}