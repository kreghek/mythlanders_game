using System.Collections.Generic;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

public sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid)
{
    public bool IsUnique { get; init; }

    public IReadOnlyCollection<IMonsterPerkPredicate> Predicates { get; init; } =
        new[]
        {
            new DefaultMonsterPerkPredicate()
        };

    /// <summary>
    /// This perk cant be rolled as location reward.
    /// </summary>
    public bool CantBeRolledAsReward { get; init; }
}