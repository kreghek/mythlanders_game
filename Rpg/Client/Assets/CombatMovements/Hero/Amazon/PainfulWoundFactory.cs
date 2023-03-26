using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Amazon;

internal class PainfulWoundFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(new ClosestInLineTargetSelector(), DamageType.Normal, new Range<int>(2, 2))
                    //new PeriodicEffect
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}