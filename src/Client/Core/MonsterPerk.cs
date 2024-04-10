using System;
using System.Collections.Generic;

using Client.Assets;

using CombatDicesTeam.Combats.CombatantStatuses;

using Microsoft.Xna.Framework;

namespace Client.Core;

public sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid)
{
    /// <summary>
    /// This perk cant be rolled as location reward.
    /// </summary>
    public bool CantBeRolledAsReward { get; init; }

    public Point IconCoords { get; init; }

    public bool IsUnique { get; init; }

    public IReadOnlyCollection<IMonsterPerkPredicate> Predicates { get; init; } =
        new[]
        {
            new DefaultMonsterPerkPredicate()
        };

    public IReadOnlyCollection<DescriptionKeyValue> Values { get; init; } =
        ArraySegment<DescriptionKeyValue>.Empty;
}