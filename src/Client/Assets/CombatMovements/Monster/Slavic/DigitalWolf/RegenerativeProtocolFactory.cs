using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;

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
                        GenericRange<int>.CreateMono(3)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                })
        );
    }
    
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new CombatMovementEffectDisplayValue("hp", ExtractStatChangingValue(combatMovementInstance, 1),
                CombatMovementEffectDisplayValueTemplate.HitPoints)
        };
    }
}