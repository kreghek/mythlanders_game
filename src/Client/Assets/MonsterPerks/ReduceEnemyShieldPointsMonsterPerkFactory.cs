﻿using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.CombatantStatusLifetimes;

using JetBrains.Annotations;

namespace Client.Assets.MonsterPerks;

[UsedImplicitly]
public sealed class ReduceEnemyShieldPointsMonsterPerkFactory : MonsterPerkFactoryBase
{
    protected override ICombatantStatusFactory CreateStatus()
    {
        return new CombatStatusFactory(source =>
            new AuraCombatantStatus(new CombatantStatusSid(nameof(PerkName)),
                new OwnerBoundCombatantEffectLifetime(),
                source,
                new CombatStatusFactory(source2 => 
                    new ModifyStatCombatantStatus(
                        new CombatantStatusSid(PerkName),
                        new TargetCombatantsBoundCombatantStatusLifetime(),
                        source2,
                        CombatantStatTypes.ShieldPoints,
                        1)),
                new EnemiesAuraTargetSelector()
            ));
    }
}