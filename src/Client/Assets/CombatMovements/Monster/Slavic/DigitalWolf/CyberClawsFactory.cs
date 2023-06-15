using Client.Assets.CombatMovements.Hero.Swordsman;
using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class CyberClawsFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AdjustPositionEffect(new SelfTargetSelector()),
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(3)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToVanguard)
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var digitalAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("DigitalWolf");

        var preparingGrinAnimation = AnimationHelper.ConvertToAnimation(digitalAnimationSet, "grin");

        var jumpAnimation = AnimationHelper.ConvertToAnimation(digitalAnimationSet, "raging-jump");

        var biteAnimation = AnimationHelper.ConvertToAnimation(digitalAnimationSet, "direct-bite");

        var jumpBackAnimation = AnimationHelper.ConvertToAnimation(digitalAnimationSet, "jump-back");

        var config = new SingleMeleeVisualizationConfig(
            new SoundedAnimation(preparingGrinAnimation, null),
            new SoundedAnimation(jumpAnimation, null),
            new SoundedAnimation(biteAnimation, null),
            new SoundedAnimation(biteAnimation, null),
            new SoundedAnimation(jumpBackAnimation, null));

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }
}