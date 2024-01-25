using System.Collections.Generic;
using System.Linq;

using Client.Engine;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

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
                    new DamageEffectWrapper(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(3)),
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

    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(CombatMovementInstance combatMovementInstance)
    {
        return new[] { 
            new CombatMovementEffectDisplayValue("damage", ExtractDamage(combatMovementInstance, 1), CombatMovementEffectDisplayValueTemplate.Damage)
        };
    }
}