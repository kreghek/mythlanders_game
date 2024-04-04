using System.Collections.Generic;

using Client.Assets.CombatMovements;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Core;

internal sealed record MonsterPerk(ICombatantStatusFactory Status, string Sid)
{
    /// <summary>
    /// This perk cant be rolled as location reward.
    /// </summary>
    public bool CantBeRolledAsReward { get; init; }

    public int IconIndex { get; init; }

    public bool IsUnique { get; init; }

    public IReadOnlyCollection<IMonsterPerkPredicate> Predicates { get; init; } =
        new[]
        {
            new DefaultMonsterPerkPredicate()
        };
}