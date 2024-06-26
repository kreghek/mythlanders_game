﻿using System.Collections.Generic;
using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class ImprovedAllDamageMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new ModifyEffectsCombatantStatus(new CombatantStatusSid(PerkName),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                1));
    }

    protected override IReadOnlyCollection<DescriptionKeyValue> CreateValues()
    {
        return new[]
        {
            new DescriptionKeyValue("damage", 1, DescriptionKeyValueTemplate.DamageModifier)
        }.ToList();
    }
}