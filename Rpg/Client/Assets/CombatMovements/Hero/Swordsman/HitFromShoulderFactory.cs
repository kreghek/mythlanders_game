using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class HitFromShoulderFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new CombatMovementIcon(0, 1);

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
                            Range<int>.CreateMono(3)),
                        new ChangePositionEffect(
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