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

namespace Client.Assets.CombatMovements.Hero.Monk;

[UsedImplicitly]
internal class NinthTrigramFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 1);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new PushToPositionEffect(
                        new StrongestClosestAllyTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),

                    new AddCombatantStatusEffect(
                        new StrongestClosestAllyTargetSelector(),
                        new CombatStatusFactory(source => {
                            return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid), new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2);
                        })),

                    new AddCombatantStatusEffect(
                        new SelfTargetSelector(),
                        new CombatStatusFactory(source => {
                            return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid), new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2);
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