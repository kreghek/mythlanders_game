using Client.Assets.States.Primitives;
using Client.Core.AnimationFrameSets;
using System.Linq;

using Client.Engine;
using Client.GameScreens.Combat.GameObjects.CommonStates;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;
using Client.Core;

namespace Client.Assets.CombatMovements.Hero.Robber;

[UsedImplicitly]
internal class ArrowsOfMoranaFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(0, 1);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new AllEnemiesTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(2))
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
        var targetArea = visualizationContext.BattlefieldInteractionContext.GetArea(Team.Cpu);

        // In the combat movement only 1 interaction.
        var materializedTargets = movementExecution.EffectImposeItems.First().MaterializedTargets;

        var startPosition = actorAnimator.GraphicRoot.Position;

        var targetCombatant =
            GetFirstTargetOrDefault(movementExecution, visualizationContext.ActorGameObject.Combatant);

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
                movementExecution.EffectImposeItems.Select(x =>
                        new InteractionDeliveryInfo(x, visualizationContext.ActorGameObject.LaunchPoint,
                            targetPosition))
                    .ToArray(),
                new EnergyArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage),
                visualizationContext.InteractionDeliveryManager)
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState,
            new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });


        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext);
    }
}