using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class SurpriseManeuverFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(2, 6);

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    /// <inheritdoc />
    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("defence", 2, DescriptionKeyValueTemplate.Defence)
        };
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
                {
                    new GroupEffect(new StrongestClosestAllyTargetSelector(),
                        new SwapPositionEffect(
                            new NullTargetSelector()
                        ),
                        new AddCombatantStatusEffect(
                            new NullTargetSelector(),
                            new CombatStatusFactory(source => new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                                new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2)))
                    ),

                    new AddCombatantStatusEffect(
                        new SelfTargetSelector(),
                        new CombatStatusFactory(source => new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                            new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2)))
                });
    }
}