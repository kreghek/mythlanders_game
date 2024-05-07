using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;

using JetBrains.Annotations;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class SurpriseManeuverFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(2, 6);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new GroupEffect(new StrongestClosestAllyTargetSelector(),
                        new SwapPositionEffect(
                            new NullTargetSelector()
                        ),
                        new AddCombatantStatusEffect(
                            new NullTargetSelector(),
                            new CombatStatusFactory(source =>
                            {
                                return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                                    new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2);
                            }))
                    ),

                    new AddCombatantStatusEffect(
                        new SelfTargetSelector(),
                        new CombatStatusFactory(source =>
                        {
                            return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                                new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2);
                        }))
                })
        );
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
}