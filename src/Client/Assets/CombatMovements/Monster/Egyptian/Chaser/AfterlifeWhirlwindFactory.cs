﻿using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Egyptian.Chaser;

internal class AfterlifeWhirlwindFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new AdjustPositionEffect(new SelfTargetSelector()),
                new DamageEffectWrapper(
                    new AllVanguardEnemiesTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(2)),
                new PushToPositionEffect(
                    new SelfTargetSelector(),
                    ChangePositionEffectDirection.ToVanguard
                )
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}