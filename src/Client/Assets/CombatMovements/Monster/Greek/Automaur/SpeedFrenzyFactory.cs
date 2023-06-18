using Core.Combats;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Greek.Automaur;

internal sealed class SpeedFrenzyFactory : SimpleCombatMovementFactoryBase
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
                    Range<int>.CreateMono(1))
            });
    }
}