﻿using System;
using System.Collections.Generic;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

public sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid)
{
    public IReadOnlyCollection<IMonsterPerkPredicate> Predicates { get; init; } =
        new[]
        {
            new DefaultMonsterPerkPredicate()
        };

    public bool IsUnique { get; init; }
}

public sealed class DefaultMonsterPerkPredicate : IMonsterPerkPredicate
{
    public bool IsApplicableTo(MonsterCombatantPrefab monsterPrefab)
    {
        return true;
    }
}