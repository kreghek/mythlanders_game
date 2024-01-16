using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.ShieldBearer;

[UsedImplicitly]
internal class SuppressiveFireFactory : CombatMovementFactoryBase
{
    private static IEffect[] CreateEffects()
    {
        return Enumerable.Range(0, 5).Select(_ => new IEffect[]
            {
                new DamageEffectWrapper(
                    new RandomEnemyTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(1)),
                new ChangeCurrentStatEffect(
                    new RandomEnemyTargetSelector(),
                    CombatantStatTypes.Resolve,
                    GenericRange<int>.CreateMono(-2))
            }
        ).SelectMany(x => x).ToArray();
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

