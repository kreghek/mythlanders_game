using System.Linq;

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
        var audioSettings = new AudioSettings();

        var launchAnimation = new SoundedAnimationFrameSet(new LinearAnimationFrameSet(Enumerable.Range(8, 2).ToArray(),
                8,
                CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8), audioSettings,
            new[]
            {
                (0, visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Gunshot))
            });

        var waitProjectileAnimation = new LinearAnimationFrameSet(Enumerable.Range(8 + 2, 2).ToArray(), 8,
            CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8);

        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext,
            new SingleDistanceVisualizationConfig(launchAnimation, waitProjectileAnimation,
                new EnergyArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage)));
    }
}