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
using Client.GameScreens;
using Microsoft.Xna.Framework.Audio;
using System;
using Client.Engine.MoveFunctions;
using System.Collections.Generic;

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
        const int TOTAL_ARROW_COUNT = 10;

        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

        // phase 1 - epic stance

        var prepareToShotAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-arrow-rain");
        var prepareToShotSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SwordPrepare);

        var prepareToShotState = CreateSoundedState(
            () => new PlayAnimationActorState(actorAnimator, prepareToShotAnimation),
            prepareToShotSoundEffect.CreateInstance());

        // phase 2 - launch rain source

        var launchRainSourceAnimation = AnimationHelper.ConvertToAnimation(animationSet, "launch-arrow-rain");
        var launchRainSourceSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.EnergoShot);

        var launchRainSourceAnimationState = CreateSoundedState(
            () => new PlayAnimationActorState(actorAnimator, launchRainSourceAnimation),
            launchRainSourceSoundEffect.CreateInstance());

        var createArrowRainState = new LaunchInteractionDeliveryState(CreateEmptyInteraction(actorAnimator),
            new ArrowRainSourceInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage),
            visualizationContext.InteractionDeliveryManager);

        var waitArrowSourceRisingAnimation = AnimationHelper.ConvertToAnimation(animationSet, "wait-rain-core");
        var arrowRainSourceRisingState = new ParallelActionState(createArrowRainState, new PlayAnimationActorState(actorAnimator, waitArrowSourceRisingAnimation));

        var launchRainSourceState = new SequentialState(launchRainSourceAnimationState, arrowRainSourceRisingState);

        // phase 3 - raining arrows

        var waitArrowRainAnimation = AnimationHelper.ConvertToAnimation(animationSet, "wait-arrow-rain");
        var waitArrowRainState = new ParallelActionState(createArrowRainState, new PlayAnimationActorState(actorAnimator, waitArrowRainAnimation));

        var items = from item in movementExecution.EffectImposeItems
                    from target in item.MaterializedTargets
                    select new InteractionDeliveryInfo(item, visualizationContext.GetCombatActor(target).InteractionPoint - new Microsoft.Xna.Framework.Vector2(400, 200), visualizationContext.GetCombatActor(target).InteractionPoint);

        var allArrowItems = new List<InteractionDeliveryInfo>(items);
        var targetArea = visualizationContext.BattlefieldInteractionContext.GetArea(Team.Cpu);
        for (var i = 0; i < TOTAL_ARROW_COUNT; i++)
        {
            var targetRandomPosition = visualizationContext.Dice.RollPoint(targetArea);
            var info = new InteractionDeliveryInfo(new CombatEffectImposeItem(combatant => { }, Array.Empty<Combatant>()),
                targetRandomPosition - new Microsoft.Xna.Framework.Vector2(400, 200),
                targetRandomPosition);

            allArrowItems.Add(info);
        }

        var rainingArrowsProjectilesState = new LaunchInteractionDeliveryState(allArrowItems,
            new EnergyArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage),
            visualizationContext.InteractionDeliveryManager);

        var rainigArrowsState = new ParallelActionState(waitArrowRainState, rainingArrowsProjectilesState);

        // total

        var subStates = new[]
        {
            prepareToShotState,
            launchRainSourceState,
            rainigArrowsState
        };

        var innerState = new SequentialState(subStates);
        return new CombatMovementScene(innerState,
            new[] { new FollowActorOperatorCameraTask(actorAnimator, () => innerState.IsComplete) });
    }

    private static InteractionDeliveryInfo[] CreateEmptyInteraction(IActorAnimator actorAnimator)
    {
        return new InteractionDeliveryInfo[]
                {
            new InteractionDeliveryInfo(new CombatEffectImposeItem(combatant => { }, Array.Empty<Combatant>()), actorAnimator.GraphicRoot.Position, actorAnimator.GraphicRoot.Position + new Microsoft.Xna.Framework.Vector2(0, -400))
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