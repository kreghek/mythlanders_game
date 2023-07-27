using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

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
        var snakeBiteSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.AspidBite);

        var hitCompleteAnimation = AnimationHelper.ConvertToAnimation(animationSet, "attack-complete");

        var backAnimation = AnimationHelper.ConvertToAnimation(animationSet, "back");

        var config = new SingleMeleeVisualizationConfig(
            new SoundedAnimation(prepareAnimation, null),
            new SoundedAnimation(chargeAnimation, null),
            new SoundedAnimation(hitAnimation, snakeBiteSoundEffect.CreateInstance()),
            new SoundedAnimation(hitCompleteAnimation, null),
            new SoundedAnimation(backAnimation, null));

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
                    GenericRange<int>.CreateMono(3)),
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