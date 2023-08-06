using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class OldGoodBrawlFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 4);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(2),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new DamageEffectWrapper(
                        new ClosestInLineTargetSelector(),
                        DamageType.Normal,
                        GenericRange<int>.CreateMono(2)),
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