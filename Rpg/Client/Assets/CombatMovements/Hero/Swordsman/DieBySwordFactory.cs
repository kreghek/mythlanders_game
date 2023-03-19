using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Swordsman;

internal class DieBySwordFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new CombatMovementIcon(0, 0);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ChangePositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(2))
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}
