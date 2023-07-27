using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Black.Agressor;

internal class GrimResolveFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new AdjustPositionEffect(new SelfTargetSelector()),
                new DamageEffect(
                    new ClosestInLineTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(2)),
                new PushToPositionEffect(
                    new ClosestInLineTargetSelector(),
                    ChangePositionEffectDirection.ToVanguard),
                new ChangeCurrentStatEffect(
                    new ClosestInLineTargetSelector(),
                    CombatantStatTypes.Resolve,
                    GenericRange<int>.CreateMono(-2))
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}