using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Amazon;

[UsedImplicitly]
internal class HuntFactory : CombatMovementFactoryBase
{
    /// <inheritdoc/>
    public override CombatMovementIcon CombatMovementIcon => new(5, 6);

    /// <inheritdoc/>
    public override CombatMovement CreateMovement()
    {
        var combatantEffectFactory = new ModifyCombatantMoveStatsCombatantEffectFactory(
            new UntilCombatantEffectMeetPredicatesLifetimeFactory(new IsAttackCombatMovePredicate()),
            CombatantMoveStats.Cost,
            -1000);
        
        var freeAttacksEffect = new AddCombatantEffectEffect(new SelfTargetSelector(), combatantEffectFactory);
        
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(4)),
                    new MarkEffect(new ClosestInLineTargetSelector(),
                        new MultipleCombatantTurnEffectLifetimeFactory(2)),
                    freeAttacksEffect
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}