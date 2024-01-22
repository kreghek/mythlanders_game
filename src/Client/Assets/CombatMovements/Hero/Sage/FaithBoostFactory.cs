using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Sage;

[UsedImplicitly]
internal class FaithBoostFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(3, 4);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new RandomLineAllyTargetSelector(),
                        new CombatStatusFactory(source =>
                            new ModifyDamageCalculatedCombatantStatus(
                                new CombatantStatusSid("SoulPower"),
                                new ToNextCombatantTurnEffectLifetime(),
                                source,
                                combatant =>
                                {
                                    return combatant.Stats
                                        .Single(x => ReferenceEquals(x.Type, CombatantStatTypes.ShieldPoints)).Value
                                        .Current;
                                })))
                })
        );
    }
}