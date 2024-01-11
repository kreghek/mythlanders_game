using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

public sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid);