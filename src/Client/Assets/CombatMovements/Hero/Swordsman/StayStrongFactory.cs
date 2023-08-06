using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

[UsedImplicitly]
internal class StayStrongFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(2, 0);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            new CombatMovementEffectConfig(
                new IEffect[]
                {
                    new ChangeStatEffect(
                        new CombatantEffectSid(Sid),
                        new SelfTargetSelector(),
                        CombatantStatTypes.Defense,
                        3,
                        new ToNextCombatantTurnEffectLifetimeFactory())
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