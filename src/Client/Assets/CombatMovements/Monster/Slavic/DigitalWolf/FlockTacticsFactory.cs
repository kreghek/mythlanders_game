using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class FlockAlphaTacticsFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AdjustPositionEffect(new SelfTargetSelector()),
                    new DamageEffectWrapper(
                        new StrongestEnemyTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(3)),
                    new ChangeCurrentStatEffect(new StrongestEnemyTargetSelector(), CombatantStatTypes.Resolve,
                        GenericRange<int>.CreateMono(-2)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToVanguard)
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
    
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new CombatMovementEffectDisplayValue("damage", 
                ExtractDamage(combatMovementInstance, 1),
                CombatMovementEffectDisplayValueTemplate.Damage),

            new CombatMovementEffectDisplayValue("resolve_damage",
                ExtractStatChangingValue(combatMovementInstance, 2) * -1,
                CombatMovementEffectDisplayValueTemplate.ResolveDamage)
        };
    }
}