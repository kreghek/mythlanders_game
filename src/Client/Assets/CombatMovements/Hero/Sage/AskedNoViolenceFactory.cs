using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Sage;

[UsedImplicitly]
internal class AskedNoViolenceFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 7);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(new SelfTargetSelector(),
                        new CombatStatusFactory(source =>
                            new HiddenThreatCombatantStatus(new CombatantStatusSid("HiddenThreat"),
                                new UntilCombatantEffectMeetPredicatesLifetime(new ICombatantStatusLifetimeExpirationCondition[]
                                {
                                    new IsAttackUsedLifetimeExpirationCondition(),
                                    new OwnerIsAttacked()
                                }), source))
                    )
                })
        );
    }
}