using System;
using System.Linq;

using Client.Assets.CombatVisualEffects;
using Client.Assets.InteractionDeliveryObjects;
using Client.Engine;
using Client.GameScreens;
using Client.GameScreens.Combat.GameObjects;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class SabotageFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 2);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToRearguard
                    ),
                    new DamageEffectWrapper(
                        new WeakestEnemyTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(3))
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
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Partisan");

        var prepareAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-rifle");

        var shotSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.CyberRifleShot);
        var shotAnimation = AnimationHelper.ConvertToAnimation(animationSet, "rifle-shot");
        var soundedShotAnimation = new SoundedAnimationFrameSet(shotAnimation,
            new[]
            {
                new AnimationFrame<IAnimationSoundEffect>(new AnimationFrameInfo(1),
                    new AnimationSoundEffect(shotSoundEffect, new AudioSettings()))
            });

        var targetCombatant =
            GetFirstTargetOrDefault(movementExecution, visualizationContext.ActorGameObject.Combatant);
        var targetPosition = visualizationContext.GetCombatActor(targetCombatant!).InteractionPoint;

        var shotEffect = new ParallelCombatVisualEffect(
            new PowderGasesCombatVisualEffect(visualizationContext.ActorGameObject.LaunchPoint, targetPosition,
                new TextureRegion2D(visualizationContext.GameObjectContentStorage.GetParticlesTexture(), new Rectangle(0, 32 * 1, 32, 32))),
            new GunFlashCombatVisualEffect(visualizationContext.ActorGameObject.LaunchPoint, 48,
                new TextureRegion2D(visualizationContext.GameObjectContentStorage.GetParticlesTexture(), new Rectangle(0, 32 * 1, 32, 32))));

        var additionalVisualEffectShotAnimation = new CombatVisualEffectAnimationFrameSet(soundedShotAnimation, visualizationContext.CombatVisualEffectManager,
            new[]
            {
                new AnimationFrame<ICombatVisualEffect>(new AnimationFrameInfo(1), shotEffect)
            });

        var waitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "rifle-wait");

        var projectileFactory = new InteractionDeliveryFactory(GetCreateProjectileFunc(visualizationContext));
        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext,
            new SingleDistanceVisualizationConfig(prepareAnimation, additionalVisualEffectShotAnimation, waitAnimation,
                projectileFactory, new AnimationFrameInfo(1)));
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

    private static Func<Vector2, Vector2, IInteractionDelivery> GetCreateProjectileFunc(
        ICombatMovementVisualizationContext visualizationContext)
    {
        return (start, target) =>
            new GunBulletProjectile(start, target, visualizationContext.GameObjectContentStorage.GetBulletGraphics());
    }
}