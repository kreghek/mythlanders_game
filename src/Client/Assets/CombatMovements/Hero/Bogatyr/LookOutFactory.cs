﻿using System.Collections.Generic;

using Client.Engine;
using Client.GameScreens;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;

using JetBrains.Annotations;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Hero.Bogatyr;

[UsedImplicitly]
internal class LookOutFactory : SimpleCombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(2, 2);

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.AutoDefense;
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new ClosestAllyInColumnTargetSelector(),
                        new CombatStatusFactory(source => new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                            new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 3))),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    )
                },
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new ClosestAllyInColumnTargetSelector(),
                        new CombatStatusFactory(source => new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                            new ToEndOfCurrentRoundEffectLifetime(), source, CombatantStatTypes.Defense, 1)))
                });
    }

    public override CombatMovementScene CreateVisualization(IActorAnimator actorAnimator,
        CombatMovementExecution movementExecution, ICombatMovementVisualizationContext visualizationContext)
    {
        var animationSet = visualizationContext.GameObjectContentStorage.GetAnimation("Bogatyr");

        var defenseAnimation = AnimationHelper.ConvertToAnimation(animationSet, "defense");
        var defenseSoundEffect =
            visualizationContext.GameObjectContentStorage.GetSkillUsageSound(GameObjectSoundType.Defence);

        return CommonCombatVisualization.CreateSelfBuffVisualization(actorAnimator, movementExecution,
            visualizationContext, defenseAnimation, defenseSoundEffect);
    }

    /// <inheritdoc />
    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("defence", ExtractStatChangingValue(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.Defence),
            new DescriptionKeyValue("auto_defence", 1, DescriptionKeyValueTemplate.Defence)
        };
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }
}