using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class InspirationalBreakthroughFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 6);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffect(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        Range<int>.CreateMono(1)),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new ModifyEffectsEffect(new AllOtherFriendlyTargetSelector(), 1)
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}