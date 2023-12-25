using System;
using System.Collections.Generic;

using CombatDicesTeam.Combats;

namespace Client.Core;

internal interface IEquipmentScheme
{
    IEquipmentSchemeMetadata? Metadata { get; }
    public EquipmentItemType RequiredResourceToLevelUp { get; }

    EquipmentSid Sid { get; }

    string GetDescription();

    IReadOnlyCollection<(ICombatantStatType, IStatModifier)> GetStatModifiers(int equipmentLevel)
    {
        return Array.Empty<(ICombatantStatType, IStatModifier)>();
    }
}