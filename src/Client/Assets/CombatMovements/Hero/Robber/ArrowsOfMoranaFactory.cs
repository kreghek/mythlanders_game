using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.ActorVisualizationStates.Primitives;
using Client.Core;
using Client.Engine;
using Client.Engine.PostProcessing;
using Client.GameScreens;
using Client.GameScreens.Combat;
using Client.GameScreens.Combat.GameObjects;
using Client.GameScreens.Combat.GameObjects.CommonStates;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using GameClient.Engine;
using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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
                    new DamageEffectWrapper(
                        new AllEnemiesTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(2))
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
        const int TOTAL_ARROW_COUNT = 10;

        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

        // phase 1 - epic stance

        var prepareToShotAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-arrow-rain");
        var prepareToShotSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SwordPrepare);
        //var prepareToShotPostEffectAnimation = new PostEffectAnimationFrameSet(prepareToShotAnimation,
        //    visualizationContext.PostEffectManager, new[]
        //    {
        //        new AnimationFrame<IPostEffect>(new AnimationFrameInfo(0),
        //            new TimeLimitedShakePostEffect(new Duration(3), new FadeOutShakeFunction(ShakePowers.Normal)))
        //    });

        var prepareToShotState = CreateSoundedState(
            () => new PlayAnimationActorState(actorAnimator, prepareToShotAnimation),
            prepareToShotSoundEffect.CreateInstance());

        // phase 2 - launch rain source

        var launchRainSourceAnimation = AnimationHelper.ConvertToAnimation(animationSet, "launch-arrow-rain");
        var launchRainSourceSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.ImpulseBowShot);
        var soundedRainSounceAnimation = new SoundedAnimationFrameSet(launchRainSourceAnimation,
            new[] {
                new AnimationFrame<IAnimationSoundEffect>(new AnimationFrameInfo(2), new AnimationSoundEffect(launchRainSourceSoundEffect, new AudioSettings()))
        });

        var waitRainSourceAnimation = AnimationHelper.ConvertToAnimation(animationSet, "wait-arrow-rain");

        var createArrowRainAndWaitState = new LaunchAndWaitInteractionDeliveryState(actorAnimator, soundedRainSounceAnimation, waitRainSourceAnimation,
            CreateEmptyRainSourceInteraction(actorAnimator),
            new ArrowRainSourceInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage),
            visualizationContext.InteractionDeliveryManager,
            new AnimationFrameInfo(2));

        // phase 3 - raining arrows

        var arrowRainOffset = new Vector2(400, 200);
        var items = from item in movementExecution.EffectImposeItems
                    select new InteractionDeliveryInfo(item,
                        visualizationContext.GetCombatActor(item.MaterializedTargets.First()).InteractionPoint -
                        arrowRainOffset,
                        visualizationContext.GetCombatActor(item.MaterializedTargets.First()).InteractionPoint);

        var allArrowItems = new List<InteractionDeliveryInfo>(items);
        var targetArea = visualizationContext.BattlefieldInteractionContext.GetArea(Team.Cpu);
        for (var i = 0; i < TOTAL_ARROW_COUNT; i++)
        {
            var targetRandomPosition = visualizationContext.Dice.RollPoint(targetArea);
            var emptyInfo = new InteractionDeliveryInfo(
                new CombatEffectImposeItem(combatant => { }, Array.Empty<TestamentCombatant>()),
                targetRandomPosition - arrowRainOffset,
                targetRandomPosition);

            allArrowItems.Add(emptyInfo);
        }

        var launchArrowsAnimation = AnimationHelper.ConvertToAnimation(animationSet, "wait-arrow-rain");
        var waitArrowsAnimation = AnimationHelper.ConvertToAnimation(animationSet, "wait-arrow-rain");
        var createRainingArrowsAndWaitState = new LaunchAndWaitInteractionDeliveryState(actorAnimator, launchArrowsAnimation, waitArrowsAnimation,
            allArrowItems,
            new RainingArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage),
            visualizationContext.InteractionDeliveryManager,
            new AnimationFrameInfo(0));

        // total

        var subStates = new[]
        {
            prepareToShotState,
            createArrowRainAndWaitState,
            createRainingArrowsAndWaitState,
            new DelayActorState(new Duration(1.5f))
        };

        var innerState = new SequentialState(subStates);

        var firstTargetCombatantGameObject = visualizationContext
                                .GetCombatActor(movementExecution.EffectImposeItems.First().MaterializedTargets.First());
        return new CombatMovementScene(innerState,
            new ICameraOperatorTask[]
            {
                new FollowActorOperatorCameraTask(actorAnimator, () => prepareToShotState.IsComplete),
                new OverviewActorsOperatorCameraTask(
                    actorAnimator,
                    firstTargetCombatantGameObject.Animator,
                    1f,
                    () => innerState.IsComplete)
            });
    }

    private static InteractionDeliveryInfo[] CreateEmptyRainSourceInteraction(IActorAnimator actorAnimator)
    {
        return new[]
        {
            new InteractionDeliveryInfo(new CombatEffectImposeItem(combatant => { }, Array.Empty<TestamentCombatant>()),
                actorAnimator.GraphicRoot.Position, actorAnimator.GraphicRoot.Position + new Vector2(0, -400))
        };
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
}