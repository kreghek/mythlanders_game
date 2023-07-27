using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

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
                    new DamageEffect(new ClosestInLineTargetSelector(), DamageType.Normal,
                        GenericRange<int>.CreateMono(2))
                    //new PeriodicEffect
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}