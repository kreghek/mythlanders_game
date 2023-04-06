using Client.Assets.States.Primitives;
using Client.Core.AnimationFrameSets;
using Client.Engine;
using Client.GameScreens.Combat.GameObjects;
using System.Linq;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Client.Assets.CombatMovements.Hero.Amazon;

[UsedImplicitly]
internal class BalticThunderFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(5, 6);

    /// <inheritdoc />
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

    /// <inheritdoc />
    public override IActorVisualizationState CreateVisualization(IActorAnimator actorAnimator, CombatMovementExecution movementExecution,
        ICombatMovementVisualizationContext visualizationContext)
    {
        var startPosition = actorAnimator.GraphicRoot.Position;
        var targetCombatant = GetFirstTargetOrDefault(movementExecution);

        var targetPosition = targetCombatant is not null
            ? visualizationContext.GetCombatActor(targetCombatant).InteractionPoint
            : startPosition;

        var subStates = new IActorVisualizationState[]
        {
            // Prepare to launch
            new PlayAnimationActorState(actorAnimator,
                new LinearAnimationFrameSet(Enumerable.Range(8, 2).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8)),
            new LaunchAndWaitInteractionDeliveryState(
                actorAnimator,
                new LinearAnimationFrameSet(Enumerable.Range(8 + 2, 2).ToArray(), 8, CommonConstants.FrameSize.X,
                    CommonConstants.FrameSize.Y, 8),
                movementExecution.EffectImposeItems.Select(x=>new InteractionDeliveryInfo(x, targetPosition)).ToArray(),
                new EnergyArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage),
                visualizationContext.InteractionDeliveryManager)
        };

        var innerState = new SequentialState(subStates);
        return innerState;
    }

    private static Combatant? GetFirstTargetOrDefault(CombatMovementExecution movementExecution)
    {
        var firstImposeItem = movementExecution.EffectImposeItems.First();

        var targetCombatUnit = firstImposeItem.MaterializedTargets.FirstOrDefault();
        return targetCombatUnit;
    }
}