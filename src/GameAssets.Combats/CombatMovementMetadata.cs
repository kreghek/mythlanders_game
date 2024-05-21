﻿namespace GameAssets.Combats;

/// <summary>
/// Meta data object to describe the game's logic over base combat engine.
/// </summary>
/// <param name="Traits">Combat movement's traits.</param>
public sealed record CombatMovementMetadata(IReadOnlyCollection<CombatMovementMetadataTrait> Traits);