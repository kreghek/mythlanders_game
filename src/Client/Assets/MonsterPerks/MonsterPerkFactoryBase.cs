using System;
using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Combats.CombatantStatuses;

using Microsoft.Xna.Framework;

namespace Client.Assets.MonsterPerks;

public abstract class MonsterPerkFactoryBase : IMonsterPerkFactory
{
    protected virtual bool CantBeRolledAsReward => false;

    protected virtual Point IconCoords => IconHelper.GetMonsterPerkIconIndex(0, 0);
    protected virtual bool IsUnique => false;
    protected string PerkName => GetType().Name[..^"MonsterPerkFactory".Length];

    protected virtual IReadOnlyCollection<IMonsterPerkPredicate> CreatePredicates()
    {
        return ArraySegment<IMonsterPerkPredicate>.Empty;
    }

    protected abstract ICombatantStatusFactory CreateStatus();

    protected virtual IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return ArraySegment<DescriptionKeyValue>.Empty;
    }


    public MonsterPerk Create()
    {
        return new MonsterPerk(CreateStatus(), PerkName)
        {
            Predicates = CreatePredicates(),
            IsUnique = IsUnique,
            CantBeRolledAsReward = CantBeRolledAsReward,
            IconCoords = IconCoords,
            Values = CreateValues()
        };
    }
}