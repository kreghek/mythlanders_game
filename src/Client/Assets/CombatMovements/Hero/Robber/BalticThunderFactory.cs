using System.Linq;

using Client.Core.AnimationFrameSets;
using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

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
        var audioSettings = new AudioSettings();

        var launchAnimation = new SoundedAnimationFrameSet(new LinearAnimationFrameSet(Enumerable.Range(8, 2).ToArray(),
                8,
                CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8), audioSettings,
            new[]
            {
                (0, visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.EnergoShot))
            });

        var waitProjectileAnimation = new LinearAnimationFrameSet(Enumerable.Range(8 + 2, 2).ToArray(), 8,
            CommonConstants.FrameSize.X, CommonConstants.FrameSize.Y, 8);

        return CommonCombatVisualization.CreateSingleDistanceVisualization(actorAnimator, movementExecution,
            visualizationContext,
            new SingleDistanceVisualizationConfig(launchAnimation, waitProjectileAnimation,
                new EnergyArrowInteractionDeliveryFactory(visualizationContext.GameObjectContentStorage)));
    }
}