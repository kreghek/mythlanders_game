using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Amazon;

internal class BringBeastDownFactory : CombatMovementFactoryBase
{
    public override CombatMovementIcon CombatMovementIcon => new(3, 7);

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new StrongestEnemyTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(4))
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}