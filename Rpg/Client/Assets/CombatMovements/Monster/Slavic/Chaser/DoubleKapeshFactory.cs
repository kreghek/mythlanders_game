using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class DoubleKapeshFactory : SimpleCombatMovementFactoryBase
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
                            Range<int>.CreateMono(2)),
                        new DamageEffect(
                            new ClosestInLineTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(2)),
                        new PushToPositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard)
                    });
    }

    protected override CombatMovementTags GetTags() => CombatMovementTags.Attack;
}
