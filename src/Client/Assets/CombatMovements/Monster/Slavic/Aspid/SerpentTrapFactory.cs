using Client.Assets.CombatMovements.Hero.Swordsman;
using Client.Engine;

using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class SerpentTrapFactory : SimpleCombatMovementFactoryBase
{
    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Aspid");

        var prepareAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-attack");

        var chargeAnimation = AnimationHelper.ConvertToAnimation(animationSet, "charge");

        var hitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "tail-attack");

        var hitCompleteAnimation = AnimationHelper.ConvertToAnimation(animationSet, "attack-complete");

        var backAnimation = AnimationHelper.ConvertToAnimation(animationSet, "back");

        var config = new SingleMeleeVisualizationConfig(
            prepareAnimation,
            chargeAnimation,
            hitAnimation,
            hitCompleteAnimation,
            backAnimation);

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }

    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new AdjustPositionEffect(new SelfTargetSelector()),
                new DamageEffect(
                    new StrongestEnemyTargetSelector(),
                    DamageType.Normal,
                    Range<int>.CreateMono(3)),
                new PushToPositionEffect(
                    new SelfTargetSelector(),
                    ChangePositionEffectDirection.ToVanguard)
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}