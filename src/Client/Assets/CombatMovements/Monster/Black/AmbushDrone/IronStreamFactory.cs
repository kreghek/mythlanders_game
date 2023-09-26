using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Black.Agressor;

internal class IronStreamFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new DamageEffectWrapper(
                    new AllLineEnemiesTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(2))
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}