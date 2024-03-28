using System;
using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Combats.CombatantStatuses;

namespace Client.Assets.MonsterPerks;

public abstract class MonsterPerkFactoryBase : IMonsterPerkFactory
{
    protected string PerkName => GetType().Name[..^"MonsterPerkFactory".Length];

    protected abstract ICombatantStatusFactory CreateStatus();


    public MonsterPerk Create()
    {
        return new MonsterPerk(CreateStatus(), PerkName)
        {
            Predicates = CreatePredicates(),
            IsUnique = IsUnique
        };
    }

    protected virtual IReadOnlyCollection<IMonsterPerkPredicate> CreatePredicates()
    {
        return ArraySegment<IMonsterPerkPredicate>.Empty;
    }

    protected virtual bool IsUnique => false;
}