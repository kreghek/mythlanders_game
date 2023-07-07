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

    IReadOnlyCollection<(ICombatantStatType, IUnitStatModifier)> GetStatModifiers(int equipmentLevel)
    {
        return Array.Empty<(ICombatantStatType, IUnitStatModifier)>();
    }
}