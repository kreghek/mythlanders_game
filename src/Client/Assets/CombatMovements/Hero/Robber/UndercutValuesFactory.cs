﻿using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Robber;

internal class UndercutValuesFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(2, 7);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(1))
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}