using System.Collections.Generic;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

public sealed record PerkMonsterCombatantPrefab(MonsterCombatantPrefab TemplatePrefab,
    IReadOnlyCollection<ICombatantStatusFactory> StartUpStatuses);