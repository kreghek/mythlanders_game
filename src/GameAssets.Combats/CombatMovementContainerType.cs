using System.Diagnostics;

using Core.Combats;

namespace GameAssets.Combats;

[DebuggerDisplay(nameof(DebugName))]
public sealed record CombatMovementContainerType(string DebugName): ICombatMovementContainerType;