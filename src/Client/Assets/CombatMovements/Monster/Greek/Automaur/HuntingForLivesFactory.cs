using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Greek.Automaur;

internal sealed class HuntingForLivesFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new AdjustPositionEffect(new SelfTargetSelector()),
                new DamageEffectWrapper(
                    new ClosestInLineTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(1))
            });
    }
}