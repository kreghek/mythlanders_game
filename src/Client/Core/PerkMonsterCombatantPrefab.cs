using System.Collections.Generic;

namespace Client.Core;

public sealed record PerkMonsterCombatantPrefab(MonsterCombatantPrefab TemplatePrefab,
    IReadOnlyCollection<MonsterPerk> Perks);