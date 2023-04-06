﻿using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

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
                    new DamageEffect(
                        new StrongestMarkedEnemyTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(4)),
                    new InterruptEffect(new SelfTargetSelector())
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}

internal class WindWheelFactory : CombatMovementFactoryBase
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
                    new DamageEffect(
                        new StrongestMarkedEnemyTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(4)),
                    new InterruptEffect(new SelfTargetSelector())
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}