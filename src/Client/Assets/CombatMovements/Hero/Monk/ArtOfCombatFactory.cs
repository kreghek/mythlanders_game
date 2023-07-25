using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;
using Core.Utils;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Monk;

[UsedImplicitly]
internal class ArtOfCombatFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(5, 3);

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
                        GenericRange<int>.CreateMono(1)),
                    new PushToPositionEffect(
                        new SelfTargetSelector(),
                        ChangePositionEffectDirection.ToVanguard
                    ),
                    new ModifyEffectsEffect(new CombatantEffectSid(Sid),
                        new AllOtherRearguardAlliesTargetSelector(),
                        1)
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}