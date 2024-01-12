using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using Core.Combats.TargetSelectors;

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
                    new ModifyEffectsEffect(
                        new CombatantStatusSid(Sid),
                        new RandomLineAllyTargetSelector(), 1)
                })
        );
    }
}