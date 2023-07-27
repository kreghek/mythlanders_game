using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;
using Core.Utils;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class InspirationalBreakthroughFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 6);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(1)),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new ModifyEffectsEffect(
                        new CombatantEffectSid(Sid),
                        new AllOtherRearguardAlliesTargetSelector(), 1)
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Partisan");

        var prepareAnimation = AnimationHelper.ConvertToAnimation(animationSet, "prepare-to-charge");
        var keepSwordSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SwordPrepare);

        var chargeAnimation = AnimationHelper.ConvertToAnimation(animationSet, "breakthrough");
        var chargeSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.ArmedMove);

        var hitAnimation = AnimationHelper.ConvertToAnimation(animationSet, "hit");
        var swordHitSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SwordSlash);

        var hitCompleteAnimation = AnimationHelper.ConvertToAnimation(animationSet, "hit-complete");

        var backAnimation = AnimationHelper.ConvertToAnimation(animationSet, "back");

        var config = new SingleMeleeVisualizationConfig(
            new SoundedAnimation(prepareAnimation, keepSwordSoundEffect.CreateInstance()),
            new SoundedAnimation(chargeAnimation, chargeSoundEffect.CreateInstance()),
            new SoundedAnimation(hitAnimation, swordHitSoundEffect.CreateInstance()),
            new SoundedAnimation(hitCompleteAnimation, null),
            new SoundedAnimation(backAnimation, null));

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }
}