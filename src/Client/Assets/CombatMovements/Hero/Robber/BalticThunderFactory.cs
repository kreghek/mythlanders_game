using System;
using System.Collections.Generic;

using Client.Assets.CombatVisualEffects;
using Client.Assets.InteractionDeliveryObjects;
using Client.Engine;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.CombatMovements.Hero.Robber;

[UsedImplicitly]
internal class BalticThunderFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 0);

    /// <inheritdoc />
    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution,
        ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

        var prepareAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-bow");

        var shotSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.ImpulseBowShot);
        var shotAnimation = AnimationHelper.ConvertToAnimation(animationSet, "bow-shot");
        var soundedShotAnimation = new SoundedAnimationFrameSet(shotAnimation,
            new[]
            {
                new AnimationFrame<IAnimationSoundEffect>(new AnimationFrameInfo(1),
                    new AnimationSoundEffect(shotSoundEffect, new AudioSettings()))
            });

        var targetPosition =
            AnimationHelper.GetTargetPositionByCombatMovementCombatant(movementExecution, visualizationContext);

        var shotEffect = new ParallelCombatVisualEffect(
            new PlasmaSparksCombatVisualEffect(visualizationContext.ActorGameObject.LaunchPoint, targetPosition,
                new TextureRegion2D(visualizationContext.GameObjectContentStorage.GetParticlesTexture(),
                    new Rectangle(0, 32 * 1, 32, 32))));

        var additionalVisualEffectShotAnimation = new CombatVisualEffectAnimationFrameSet(soundedShotAnimation,
            visualizationContext.CombatVisualEffectManager,
            new[]
            {
                new AnimationFrame<ICombatVisualEffect>(new AnimationFrameInfo(3), shotEffect)
            });

        var waitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "bow-wait");

        var projectileFactory = new InteractionDeliveryFactory(GetCreateProjectileFunc(visualizationContext));
        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext,
            new SingleDistanceVisualizationConfig(prepareAnimation, additionalVisualEffectShotAnimation, waitAnimation,
                projectileFactory, new AnimationFrameInfo(1)));
    }

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage", ExtractDamage(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.Damage)
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Ranged;
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(3);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
        {
            new DamageEffectWrapper(
                new ClosestInLineTargetSelector(),
                DamageType.Normal,
                GenericRange<int>.CreateMono(3))
        });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }

    private static Func<Vector2, Vector2, IInteractionDelivery> GetCreateProjectileFunc(
        ICombatMovementVisualizationContext visualizationContext)
    {
        return (start, target) =>
            new EnergyArrowProjectile(start, target, visualizationContext.GameObjectContentStorage.GetBulletGraphics());
    }
}