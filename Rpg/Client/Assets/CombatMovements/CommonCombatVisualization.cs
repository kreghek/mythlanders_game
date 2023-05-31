using System.Collections.Generic;
using System.Linq;

using Client.Assets.CombatMovements.Hero.Amazon;
using Client.Assets.CombatMovements.Hero.Swordsman;
using Client.Assets.States.Primitives;
using Client.Core.AnimationFrameSets;
using Client.Engine;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;

using Core.Combats;

using Microsoft.Xna.Framework;

using Rpg.Client.GameScreens.Combat.GameObjects;
using Rpg.Client.GameScreens.Combat.GameObjects.CommonStates;

namespace Client.Assets.CombatMovements;

internal static class CommonCombatVisualization
{
    public static CombatMovementScene CreateSingleDistanceVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
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
        return new CombatMovementScene(innerState, new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });
    }

    public static CombatMovementScene CreateSingleMeleeVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext,
        SingleMeleeVisualizationConfig config)
    {
        var skillAnimationInfo = new SkillAnimationInfo
        {
            Items = new[]
            {
                new SkillAnimationInfoItem
                {
                    Duration = 0.75f,
                    //HitSound = hitSound,
                    Interaction = () =>
                        Interaction(movementExecution.EffectImposeItems),
                    InteractTime = 0
                }
            }
        };

        var startPosition = actorAnimator.GraphicRoot.Position;
        var targetCombatant =
            GetFirstTargetOrDefault(movementExecution, visualizationContext.ActorGameObject.Combatant);

        Vector2 targetPosition;

        if (targetCombatant is not null)
        {
            var targetGameObject = visualizationContext.GetCombatActor(targetCombatant);

            targetPosition = targetGameObject.MeleeHitOffset;
        }
        else
        {
            targetPosition = actorAnimator.GraphicRoot.Position;
        }

        var subStates = new IActorVisualizationState[]
        {
            new PlayAnimationActorState(actorAnimator, config.PrepareMovementAnimation),
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, targetPosition),
                config.CombatMovementAnimation),
            new DirectInteractionState(actorAnimator, skillAnimationInfo, config.HitAnimation),
            new PlayAnimationActorState(actorAnimator, config.HitCompleteAnimation),
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, startPosition),
                config.BackAnimation)
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState, new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });
    }

    private static Combatant? GetFirstTargetOrDefault(CombatMovementExecution movementExecution,
        Combatant actorCombatant)
    {
        var firstImposeItem =
            movementExecution.EffectImposeItems.FirstOrDefault(x =>
                x.MaterializedTargets.All(t => t != actorCombatant));
        if (firstImposeItem is null)
        {
            return null;
        }

        var targetCombatUnit = firstImposeItem.MaterializedTargets.FirstOrDefault(t => t != actorCombatant);
        return targetCombatUnit;
    }

    private static void Interaction(IReadOnlyCollection<CombatEffectImposeItem> effectImposeItems)
    {
        foreach (var effectImposeItem in effectImposeItems)
        {
            foreach (var target in effectImposeItem.MaterializedTargets)
            {
                effectImposeItem.ImposeDelegate(target);
            }
        }
    }
}