﻿using System.Collections.Generic;

using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

[UsedImplicitly]
internal class HitFromShoulderFactory : SimpleCombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(1, 0);

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var swordsmanAnimationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Swordsman");

        var keepSwordStrongerAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "prepare-sword");
        var keepSwordSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SwordPrepare);

        var chargeAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "charge");
        var chargeSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.ArmedMove);

        var hitAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "hit");
        var swordHitSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.SwordSlash);

        var hitCompleteAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "hit-complete");

        var backAnimation = AnimationHelper.ConvertToAnimation(swordsmanAnimationSet, "back");

        var config = new SingleMeleeVisualizationConfig(
            new SoundedAnimation(keepSwordStrongerAnimation, keepSwordSoundEffect.CreateInstance()),
            new SoundedAnimation(chargeAnimation, chargeSoundEffect.CreateInstance()),
            new SoundedAnimation(hitAnimation, swordHitSoundEffect.CreateInstance()),
            new SoundedAnimation(hitCompleteAnimation, null),
            new SoundedAnimation(backAnimation, null));

        return CommonCombatVisualization.CreateSingleMeleeVisualization(actorAnimator, movementExecution,
            visualizationContext, config);
    }

    /// <inheritdoc />
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new CombatMovementEffectDisplayValue("damage", ExtractDamage(combatMovementInstance, 0),
                CombatMovementEffectDisplayValueTemplate.Damage)
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Melee;
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(3);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
        {
            new DamageEffectWrapper(
                new ClosestInLineTargetSelector(),
                DamageType.Normal,
                GenericRange<int>.CreateMono(3)),
            new PushToPositionEffect(
                new SelfTargetSelector(),
                ChangePositionEffectDirection.ToVanguard
            )
        });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}