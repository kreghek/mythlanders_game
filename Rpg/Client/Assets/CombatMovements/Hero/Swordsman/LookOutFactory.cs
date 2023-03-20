using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class LookOutFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(2, 2);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new ChangeStatEffect(new ClosestAllyInColumnTargetSelector(),
                        UnitStatType.Defense,
                        3,
                        typeof(ToNextCombatantTurnEffectLifetime)),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    )
                },
                new IEffect[]
                {
                    new ChangeStatEffect(new SelfTargetSelector(),
                        UnitStatType.Defense,
                        1,
                        typeof(ToEndOfCurrentRoundEffectLifetime))
                })
        )
        {
            Tags = CombatMovementTags.AutoDefense
        };
    }
}