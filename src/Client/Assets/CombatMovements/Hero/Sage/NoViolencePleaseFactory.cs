using System.Linq;

using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Sage;

[UsedImplicitly]
internal class NoViolencePleaseFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(3, 6);

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
                        new CombatStatusFactory(source=>
                            new ModifyStatCombatantStatus(
                                new CombatantStatusSid("SoulArmor"),
                                new ToNextCombatantTurnEffectLifetime(),
                                source,
                                CombatantStatTypes.ShieldPoints,
                                status =>
                                {
                                    return ((CombatMovementCombatantStatusSource)status.Source).Actor.Statuses.Count(x => (CombatantStatusSid)x.Sid == new CombatantStatusSid("PartOfSoul"));
                                })))
                })
        );
    }
}