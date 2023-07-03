using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

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
                        new CombatantEffectSid(Sid),
                        new StrongestClosestAllyTargetSelector(),
                        UnitStatType.Defense,
                        2,
                        new ToNextCombatantTurnEffectLifetimeFactory()),
                    new ChangeStatEffect(
                        new CombatantEffectSid(Sid),
                        new SelfTargetSelector(),
                        UnitStatType.Defense,
                        2,
                        new ToNextCombatantTurnEffectLifetimeFactory())
                })
        );
    }
}