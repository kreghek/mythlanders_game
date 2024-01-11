using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using CombatDicesTeam.GenericRanges;
using Core.Combats.TargetSelectors;
using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Hero.Sage;

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
                    new DamageEffectWrapper(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(1)),
                    new InterruptEffect(new SelfTargetSelector())
                })
        );
    }
}