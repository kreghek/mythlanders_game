using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.Chaser;

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
                    GenericRange<int>.CreateMono(3)),
                new PushToPositionEffect(
                    new SelfTargetSelector(),
                    ChangePositionEffectDirection.ToVanguard)
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}