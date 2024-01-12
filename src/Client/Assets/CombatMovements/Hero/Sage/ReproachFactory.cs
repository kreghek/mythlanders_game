using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

using Core.Combats.CombatantStatuses;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Sage;

[UsedImplicitly]
internal class ReproachFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 7);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(new SelfTargetSelector(),
                        new DelegateCombatStatusFactory(() =>
                            new SoulTakerCombatantStatus(new CombatantStatusSid("SoulTaker"),
                                new CombatantStatusSid("PartOfSoul"), new UntilCombatantEffectMeetPredicatesLifetime(new ICombatMovePredicate[]
                                {
                                    new IsAttackCombatMovePredicate()
                                })))
                    )
                })
        );
    }
}

