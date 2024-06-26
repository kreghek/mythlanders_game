﻿using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Hero.Amazon;

internal class BringBeastDownFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(3, 7);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffectWrapper(
                        new StrongestMarkedEnemyTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(4)),
                    new InterruptEffect(new SelfTargetSelector())
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}