using System.Collections.Generic;

namespace Client.Core;

public sealed record CombatSource(IReadOnlyCollection<PerkMonsterCombatantPrefab> Monsters, CombatReward Reward);