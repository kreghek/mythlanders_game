using System;
using System.Collections.Generic;

using Core.Combats;

namespace Client.Core;

internal interface IEquipmentScheme
{
    IEquipmentSchemeMetadata? Metadata { get; }
    public EquipmentItemType RequiredResourceToLevelUp { get; }

    EquipmentSid Sid { get; }

    string GetDescription();

    IReadOnlyCollection<(UnitStatType, IUnitStatModifier)> GetStatModifiers(int equipmentLevel)
    {
        return Array.Empty<(UnitStatType, IUnitStatModifier)>();
    }
}