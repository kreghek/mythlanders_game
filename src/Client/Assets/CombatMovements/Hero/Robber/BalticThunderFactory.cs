using Client.Engine;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;
using Core.Combats.CombatantStatus;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Robber;

[UsedImplicitly]
internal class BalticThunderFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 0);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        var combatantEffectFactory = new ModifyCombatantMoveStatsCombatantEffectFactory(
            new CombatantEffectSid(Sid),
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

    /// <inheritdoc />
    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution,
        ICombatMovementVisualizationContext visualizationContext)
    {
        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext);
    }
}