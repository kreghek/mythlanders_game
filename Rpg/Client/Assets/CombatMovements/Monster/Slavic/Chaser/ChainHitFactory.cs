using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class ChainHitFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
                    new IEffect[]
                    {
                        new AdjustPositionEffect(new SelfTargetSelector()),
                        new DamageEffect(
                            new StrongestEnemyTargetSelector(),
                            DamageType.Normal,
                            Range<int>.CreateMono(3)),
                        new PushToPositionEffect(
                            new SelfTargetSelector(),
                            ChangePositionEffectDirection.ToVanguard)
                    });
    }

    protected override CombatMovementTags GetTags() => CombatMovementTags.Attack;
}
