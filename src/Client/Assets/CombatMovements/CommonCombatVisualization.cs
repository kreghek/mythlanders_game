using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;
using Client.Engine;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.CommonStates;

using CombatDicesTeam.Combats;

using GameClient.Engine;
using GameClient.Engine.Animations;
using GameClient.Engine.MoveFunctions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Client.Assets.CombatMovements;

internal static class CommonCombatVisualization
{
    public static CombatMovementScene CreateSelfBuffVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext,
        IAnimationFrameSet animation, SoundEffect defenseSoundEffect)
    {
        var skillAnimationInfo = new SkillAnimationInfo
        {
            Items = new[]
            {
                new SkillAnimationStage
                {
                    Duration = 0.75f,
                    HitSound = defenseSoundEffect.CreateInstance(),
                    Interaction = () =>
                        Interaction(movementExecution.EffectImposeItems),
                    InteractTime = 0
                }
            }
        };

        var subStates = new IActorVisualizationState[]
        {
            new DirectInteractionState(actorAnimator, skillAnimationInfo, animation)
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState,
            new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });
    }

    public static CombatMovementScene CreateSingleDistanceVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext,
        SingleDistanceVisualizationConfig config)
    {
        var targetCombatant =
            GetFirstTargetOrDefault(movementExecution, visualizationContext.ActorGameObject.Combatant);

        var targetPosition = targetCombatant is not null
            ? visualizationContext.GetCombatActor(targetCombatant).InteractionPoint
            : visualizationContext.BattlefieldInteractionContext
                .GetArea(visualizationContext.ActorGameObject.Combatant.IsPlayerControlled ? Team.Cpu : Team.Player)
                .Center.ToVector2();

        var targetAnimator = targetCombatant is not null
            ? visualizationContext.GetCombatActor(targetCombatant).Animator
            : actorAnimator;

        var moveToPositionActorState = new MoveToPositionActorState(actorAnimator,
            () => new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position,
                visualizationContext.BattlefieldInteractionContext.GetCombatantPosition(visualizationContext
                    .ActorGameObject.Combatant)),
            config.WaitAnimation,
            new Duration(1));

        var subStates = new IActorVisualizationState[]
        {
            // Prepare to launch
            new PlayAnimationActorState(actorAnimator,
                config.PrepareAnimation),
            new LaunchAndWaitInteractionDeliveryState(
                actorAnimator,
                config.LaunchProjectileAnimation,
                config.WaitAnimation,
                movementExecution.EffectImposeItems.Select(x =>
                        new InteractionDeliveryInfo(x, visualizationContext.ActorGameObject.LaunchPoint,
                            targetPosition))
                    .ToArray(),
                config.DeliveryFactory,
                visualizationContext.InteractionDeliveryManager,
                config.LaunchFrame),
            moveToPositionActorState
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState,
            new ICameraOperatorTask[]
            {
                new FollowActorOperatorCameraTask(actorAnimator, () => subStates[0].IsComplete),
                new OverviewActorsOperatorCameraTask(actorAnimator, targetAnimator, 1f, () => innerState.IsComplete)
            });
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

        var targetCombatant =
            GetFirstTargetOrDefault(movementExecution, visualizationContext.ActorGameObject.Combatant);

        Vector2 targetPosition;

        if (targetCombatant is not null)
        {
            var targetGameObject = visualizationContext.GetCombatActor(targetCombatant);

            var offset2 = GetCombatMovementVisualizationOffset();
            var offset = visualizationContext.ActorGameObject.Combatant.IsPlayerControlled ? offset2 : -offset2;

            targetPosition = targetGameObject.MeleeHitOffset - offset;
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

        var moveFunction = !visualizationContext
                        .ActorGameObject.Combatant.IsDead ? new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position,
                    visualizationContext.BattlefieldInteractionContext.GetCombatantPosition(visualizationContext
                        .ActorGameObject.Combatant)) : new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, actorAnimator.GraphicRoot.Position);

        var subStates = new[]
        {
            prepareActorState,
            chargeActorState,
            new DirectInteractionState(actorAnimator, skillAnimationInfo, config.HitAnimation.Animation),
            new PlayAnimationActorState(actorAnimator, config.HitCompleteAnimation.Animation),
            new MoveToPositionActorState(actorAnimator,
                () => moveFunction,
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

    private static Vector2 GetCombatMovementVisualizationOffset()
    {
        return Vector2.UnitX * (32 + 32);
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

    private static void Interaction(IEnumerable<CombatEffectImposeItem> effectImposeItems)
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