using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class SabotageFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(0, 0);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new WeakestEnemyTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(3)),
                    new ChangePositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToRearguard
                    )
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}