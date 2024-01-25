using System.Collections.Generic;

namespace GameAssets.Combats;

public sealed record CombatMovementMetadata(IReadOnlyCollection<CombatMovementMetadataTrait> Traits);
