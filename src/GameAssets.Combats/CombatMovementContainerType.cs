using System.Diagnostics;

using CombatDicesTeam.Combats;

namespace GameAssets.Combats;

[DebuggerDisplay(nameof(DebugName))]
public sealed record CombatMovementContainerType(string DebugName): ICombatMovementContainerType;