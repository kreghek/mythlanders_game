﻿using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class RegenerativeProtocolFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
                {
                    new ChangeCurrentStatEffect(new SelfTargetSelector(), CombatantStatTypes.HitPoints,
                        GenericRange<int>.CreateMono(0)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                });
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("hp", ExtractStatChangingValue(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.HitPoints)
        };
    }
}