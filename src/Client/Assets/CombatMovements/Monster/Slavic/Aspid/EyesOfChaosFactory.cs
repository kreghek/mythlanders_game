using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class EyesOfChaosFactory : SimpleCombatMovementFactoryBase
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
    
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(CombatMovementInstance combatMovementInstance)
    {
        return new[] { 
            new CombatMovementEffectDisplayValue("damage", ExtractDamage(combatMovementInstance, 1), CombatMovementEffectDisplayValueTemplate.Damage)
        };
    }
}