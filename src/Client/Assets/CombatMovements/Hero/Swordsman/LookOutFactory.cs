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
                    new ChangeStatEffect(
                        new CombatantEffectSid(Sid),
                        new ClosestAllyInColumnTargetSelector(),
                        CombatantStatTypes.Defense,
                        3,
                        new ToNextCombatantTurnEffectLifetimeFactory()),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    )
                },
                new IEffect[]
                {
                    new ChangeStatEffect(
                        new CombatantEffectSid(Sid),
                        new SelfTargetSelector(),
                        CombatantStatTypes.Defense,
                        1,
                        new ToEndOfCurrentRoundEffectLifetimeFactory())
                })
        )
        {
            Tags = CombatMovementTags.AutoDefense
        };
    }
}