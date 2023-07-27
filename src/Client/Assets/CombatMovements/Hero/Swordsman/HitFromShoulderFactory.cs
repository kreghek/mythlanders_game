using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;
using Core.Utils;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class HitFromShoulderFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(1, 0);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(3)),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    )
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}