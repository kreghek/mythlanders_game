using System.Collections.Generic;

using Client.Engine;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class CyberClawsFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
                {
                    new AdjustPositionEffect(new SelfTargetSelector()),
                    new DamageEffectWrapper(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(3)),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToVanguard)
                });
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Melee;
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
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

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage", ExtractDamage(combatMovementInstance, 1),
                DescriptionKeyValueTemplate.Damage)
        };
    }
}