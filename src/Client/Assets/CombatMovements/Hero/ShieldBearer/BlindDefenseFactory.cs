using System.Linq;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Hero.ShieldBearer;

[UsedImplicitly]
internal class BlindDefenseFactory : CombatMovementFactoryBase
{
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

    private IEffect[] CreateEffects()
    {
        return Enumerable.Range(0, 5).Select(_ =>
            new DamageEffectWrapper(
                new RandomEnemyTargetSelector(),
                DamageType.Normal,
                GenericRange<int>.CreateMono(1))
        ).Cast<IEffect>().Union(new[]
        {
            new AddCombatantStatusEffect(
                            new SelfTargetSelector(),
                            new CombatStatusFactory(source => {
                                return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid), new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 3);
                            }))
        }).ToArray();
    }
}