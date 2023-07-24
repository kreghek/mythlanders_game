using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Assets.CombatMovements.Hero.Robber;
using Client.Core.AnimationFrameSets;
using Client.Engine;
using Client.Engine.MoveFunctions;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.CommonStates;

using Core.Combats;

using GameAssets.Combats;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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

        var launchAnimation = new LinearAnimationFrameSet(Enumerable.Range(8, 2).ToArray(), 8,
            CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8);

        var waitProjectileAnimation = new LinearAnimationFrameSet(Enumerable.Range(8 + 2, 2).ToArray(), 8,
            CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8);

        var subStates = new IActorVisualizationState[]
        {
            // Prepare to launch
            new PlayAnimationActorState(actorAnimator,
                launchAnimation),
            new LaunchAndWaitInteractionDeliveryState(
                actorAnimator,
                waitProjectileAnimation,
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
    }

    public static CombatMovementScene CreateSingleMeleeVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext,
        SingleMeleeVisualizationConfig config)
    {
        var skillAnimationInfo = new SkillAnimationInfo
        {
            Items = new[]
            {
                new SkillAnimationStage
                {
                    Duration = 0.75f,
                    HitSound = config.HitAnimation.Sound,
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

        var prepareActorState = CreateSoundedState(
            () => new PlayAnimationActorState(actorAnimator, config.PrepareMovementAnimation.Animation),
            config.PrepareMovementAnimation.Sound);

        var chargeActorState = CreateSoundedState(
            () => new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, targetPosition),
                config.CombatMovementAnimation.Animation), config.CombatMovementAnimation.Sound);

        var subStates = new[]
        {
            prepareActorState,
            chargeActorState,
            new DirectInteractionState(actorAnimator, skillAnimationInfo, config.HitAnimation.Animation),
            new PlayAnimationActorState(actorAnimator, config.HitCompleteAnimation.Animation),
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, startPosition),
                config.BackAnimation.Animation)
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState,
            new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });
    }

    private static IActorVisualizationState CreateSoundedState(Func<IActorVisualizationState> baseStateFactory,
        SoundEffectInstance? soundEffect)
    {
        var baseActorState = baseStateFactory();
        if (soundEffect is not null)
        {
            baseActorState = new ParallelActionState(new PlaySoundEffectActorState(soundEffect), baseActorState);
        }

        return baseActorState;
    }

    private static ICombatant? GetFirstTargetOrDefault(CombatMovementExecution movementExecution,
        ICombatant actorCombatant)
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