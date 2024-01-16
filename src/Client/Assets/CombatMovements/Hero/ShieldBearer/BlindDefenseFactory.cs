using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.ShieldBearer;

[UsedImplicitly]
internal class BlindDefenseFactory : CombatMovementFactoryBase
{
    private static IEffect[] CreateEffects()
    {
        return Enumerable.Range(0, 5).Select(_ =>
            new DamageEffectWrapper(
                new RandomEnemyTargetSelector(),
                DamageType.Normal,
                GenericRange<int>.CreateMono(1))
        ).Cast<IEffect>().ToArray();
    }

    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(3),
            CombatMovementEffectConfig.Create(
                CreateEffects()
                )
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }
}