﻿using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Black.Aggressor;

internal class FatalBlowFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new AdjustPositionEffect(new SelfTargetSelector()),
                new DamageEffectWrapper(
                    new ClosestInLineTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(2)),
                new PushToPositionEffect(
                    new ClosestInLineTargetSelector(),
                    ChangePositionEffectDirection.ToVanguard),
                new ChangeCurrentStatEffect(
                    new ClosestInLineTargetSelector(),
                    CombatantStatTypes.Resolve,
                    GenericRange<int>.CreateMono(-2))
            });
    }
    
    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Ranged;
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}