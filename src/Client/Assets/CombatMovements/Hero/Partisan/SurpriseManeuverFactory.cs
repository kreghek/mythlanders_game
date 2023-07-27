using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class SurpriseManeuverFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(2, 6);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new GroupEffect(new StrongestClosestAllyTargetSelector(),
                        new SwapPositionEffect(
                            new NullTargetSelector()
                        ),
                        new ChangeStatEffect(
                            new CombatantEffectSid(Sid),
                            new NullTargetSelector(),
                            CombatantStatTypes.Defense,
                            2,
                            new ToNextCombatantTurnEffectLifetimeFactory())
                    ),
                    new ChangeStatEffect(
                        new CombatantEffectSid(Sid),
                        new SelfTargetSelector(),
                        CombatantStatTypes.Defense,
                        2,
                        new ToNextCombatantTurnEffectLifetimeFactory())
                })
        );
    }
}