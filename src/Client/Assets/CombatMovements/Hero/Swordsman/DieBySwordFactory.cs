using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class DieBySwordFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(0, 0);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(2))
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var swordsmanAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Swordsman");

        var keepSwordStrongerAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "prepare-sword");

        var chargeAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "charge");

        var hitAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "hit");

        var hitCompleteAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "hit-complete");

        var backAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "back");

        var config = new SingleMeleeVisualizationConfig(
            keepSwordStrongerAnimation,
            chargeAnimation,
            hitAnimation,
            hitCompleteAnimation,
            backAnimation);

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }
}
