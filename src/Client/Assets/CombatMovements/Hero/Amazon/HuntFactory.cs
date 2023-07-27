using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Amazon;

[UsedImplicitly]
internal class HuntFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(5, 6);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        var combatantEffectFactory = new ModifyCombatantMoveStatsCombatantStatusFactory(
            new CombatantEffectSid(Sid),
            new UntilCombatantEffectMeetPredicatesLifetimeFactory(new IsAttackCombatMovePredicate()),
            CombatantMoveStats.Cost,
            -1000);

        var freeAttacksEffect = new AddCombatantStatusEffect(new SelfTargetSelector(), combatantEffectFactory);

        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(4)),
                    new MarkEffect(new ClosestInLineTargetSelector(),
                        new MultipleCombatantTurnEffectLifetimeFactory(2)),
                    freeAttacksEffect
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}