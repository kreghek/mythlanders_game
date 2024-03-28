using System;
using System.Collections.Generic;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

public sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid)
{
    public IReadOnlyCollection<IMonsterPerkPredicate> Predicates { get; init; } =
        ArraySegment<IMonsterPerkPredicate>.Empty;

    public bool IsUnique { get; init; }
};