using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Sage;

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
                    new InterruptEffect(new ClosestInLineTargetSelector())
                })
        );
    }
}