using Client.Core;

namespace Client.Assets.StageItems;

internal sealed record CombatMetadata(MonsterCombatantPrefab MonsterLeader,
    CombatEstimateDifficulty EstimateDifficulty);
