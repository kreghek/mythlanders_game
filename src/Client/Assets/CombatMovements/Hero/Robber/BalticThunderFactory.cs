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

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using GameClient.Engine.Animations;
using GameClient.Engine.CombatVisualEffects;

using JetBrains.Annotations;

using Microsoft.Xna.Framework;

using MonoGame.Extended.TextureAtlases;

namespace Client.Assets.CombatMovements.Hero.Robber;

[UsedImplicitly]
internal class BalticThunderFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 0);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffectWrapper(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(4))
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
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

        var prepareAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-bow");

        var shotSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.CyberRifleShot);
        var shotAnimation = AnimationHelper.ConvertToAnimation(animationSet, "bow-shot");
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
                new TextureRegion2D(visualizationContext.GameObjectContentStorage.GetParticlesTexture(),
                    new Rectangle(0, 32 * 1, 32, 32))),
            new GunFlashCombatVisualEffect(visualizationContext.ActorGameObject.LaunchPoint, 48,
                new TextureRegion2D(visualizationContext.GameObjectContentStorage.GetParticlesTexture(),
                    new Rectangle(0, 32 * 1, 32, 32))));

        var additionalVisualEffectShotAnimation = new CombatVisualEffectAnimationFrameSet(soundedShotAnimation,
            visualizationContext.CombatVisualEffectManager,
            new[]
            {
                new AnimationFrame<ICombatVisualEffect>(new AnimationFrameInfo(1), shotEffect)
            });

        var waitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "bow-wait");

        var projectileFactory = new InteractionDeliveryFactory(GetCreateProjectileFunc(visualizationContext));
        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext,
            new SingleDistanceVisualizationConfig(prepareAnimation, additionalVisualEffectShotAnimation, waitAnimation,
                projectileFactory, new AnimationFrameInfo(1)));
    }

    private static Func<Vector2, Vector2, IInteractionDelivery> GetCreateProjectileFunc(
        ICombatMovementVisualizationContext visualizationContext)
    {
        return (start, target) =>
            new EnergyArrowProjectile(start, target, visualizationContext.GameObjectContentStorage.GetBulletGraphics());
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
}