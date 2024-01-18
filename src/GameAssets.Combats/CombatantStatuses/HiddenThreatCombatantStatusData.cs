using CombatDicesTeam.Combats;

namespace GameAssets.Combats.CombatantStatuses;

internal sealed record HiddenThreatCombatantStatusData(ICombatant Owner, HiddenThreatStatModifier Modifier, CombatEngineBase Combat);