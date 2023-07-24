﻿using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;
using Core.Utils;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class RegenerativeProtocolFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ChangeCurrentStatEffect(new SelfTargetSelector(), CombatantStatTypes.HitPoints,
                        Range<int>.CreateMono(3)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                })
        );
    }
}