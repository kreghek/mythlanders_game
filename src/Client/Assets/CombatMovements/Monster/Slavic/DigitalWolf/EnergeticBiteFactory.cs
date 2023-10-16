using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Engine;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.CommonStates;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using GameClient.Engine.MoveFunctions;

using Microsoft.Xna.Framework;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class EnergeticBiteFactory : CombatMovementFactoryBase
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
                        new MostShieldChargedEnemyTargetSelector(),
                        DamageType.ShieldsOnly,
                        GenericRange<int>.CreateMono(3)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("DigitalWolf");

        var preparingGrinAnimation = AnimationHelper.ConvertToAnimation(animationSet, "grin");

        var jumpAnimation = AnimationHelper.ConvertToAnimation(animationSet, "digital-jump");

        var chargeAnimation = AnimationHelper.ConvertToAnimation(animationSet, "digital-charge");

        var biteAnimation = AnimationHelper.ConvertToAnimation(animationSet, "digital-bite");

        var waitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "digital-wait");

        var jumpBackAnimation = AnimationHelper.ConvertToAnimation(animationSet, "jump-back");

        var skillAnimationInfo = new SkillAnimationInfo
        {
            Items = new[]
            {
                new SkillAnimationStage
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
            new PlayAnimationActorState(actorAnimator, preparingGrinAnimation),

            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position,
                    actorAnimator.GraphicRoot.Position + new Vector2(50, -256)), jumpAnimation),
            // TODO Launch particle system - raining cyan digital particles
            new DelayActorState(new Duration(1)),

            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(targetPosition + new Vector2(-50, -256), targetPosition), chargeAnimation),
            new DirectInteractionState(actorAnimator, skillAnimationInfo, biteAnimation),
            new PlayAnimationActorState(actorAnimator, waitAnimation),
            new MoveToPositionActorState(actorAnimator,
                new SlowDownMoveFunction(actorAnimator.GraphicRoot.Position, startPosition), jumpBackAnimation)
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState,
            new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });
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