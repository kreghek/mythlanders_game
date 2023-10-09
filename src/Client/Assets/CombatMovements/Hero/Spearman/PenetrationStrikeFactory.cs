using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Hero.Spearman;

internal class PenetrationStrikeFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
        new CombatMovementCost(2),
        CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new DamageEffectWrapper(
                    new ClosestInLineTargetSelector(),
                    DamageType.ShieldsOnly,
                    GenericRange<int>.CreateMono(3))
            })
    )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}

