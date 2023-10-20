using Client.Assets.CombatMovements.Hero.Robber;
using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using GameClient.Engine.Animations;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class BlankShotFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(3, 6);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffectWrapper(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(2)),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    )
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

        var prepareAnimation = AnimationHelper.ConvertToAnimation(animationSet, "uzi-prepare");

        var shotSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Gunshot);
        var shotAnimation = AnimationHelper.ConvertToAnimation(animationSet, "uzi-shot");
        var soundedShotAnimation = new SoundedAnimationFrameSet(shotAnimation,
            new[]
            {
                new GameClient.Engine.Animations.AnimationSoundEffect(new AnimationFrameInfo(1),
                    new AnimationSoundEffect(shotSoundEffect, new AudioSettings()))
            });

        var waitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "uzi-wait");

        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext,
            new SingleDistanceVisualizationConfig(prepareAnimation, soundedShotAnimation, waitAnimation,
                new EnergyArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage)));
    }
}