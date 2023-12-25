using System.Collections.Generic;

namespace Client.Core;

public sealed record CombatSource(IReadOnlyCollection<MonsterCombatantPrefab> Monsters, CombatReward Reward);