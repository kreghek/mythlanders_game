using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;

using JetBrains.Annotations;

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
                    new ChangeStatEffect(
                        new CombatantStatusSid(Sid),
                        new StrongestClosestAllyTargetSelector(),
                        CombatantStatTypes.Defense,
                        2,
                        new ToNextCombatantTurnEffectLifetimeFactory()),
                    new ChangeStatEffect(
                        new CombatantStatusSid(Sid),
                        new SelfTargetSelector(),
                        CombatantStatTypes.Defense,
                        2,
                        new ToNextCombatantTurnEffectLifetimeFactory())
                })
        );
    }

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("defence", ExtractStatChangingValue(combatMovementInstance, 1),
                DescriptionKeyValueTemplate.Defence)
        };
    }
}