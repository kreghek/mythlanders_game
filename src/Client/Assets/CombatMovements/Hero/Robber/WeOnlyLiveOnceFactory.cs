using System.Collections.Generic;

using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Robber;

[UsedImplicitly]
internal class WeOnlyLiveOnceFactory : SimpleCombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(4, 7);

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Robber");

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

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage", ExtractDamage(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.Damage),
            new DescriptionKeyValue("duration", 3, DescriptionKeyValueTemplate.RoundDuration),
            new DescriptionKeyValue("bleed_damage", 1,
                DescriptionKeyValueTemplate.HitPointsDamage)
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Melee;
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
        {
            new DamageEffectWrapper(
                new ClosestInLineTargetSelector(),
                DamageType.Normal,
                GenericRange<int>.CreateMono(1))
        });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}