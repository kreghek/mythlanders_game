using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.Combats;
using CombatDicesTeam.GenericRanges;
using Core.Combats.TargetSelectors;
using GameAssets.Combats.CombatMovementEffects;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Client.Assets.CombatMovements.Hero.Priest;

[UsedImplicitly]
internal class DarkLightningFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(3, 5);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                new DamageEffectWrapper(
                    new ClosestInLineTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(1)),
                new InterruptEffect(new ClosestInLineTargetSelector())
                })
        )
        {
            Tags = CombatMovementTags.Attack
        };
    }

    /// <inheritdoc />
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(CombatMovementInstance combatMovementInstance)
    {
        return new CombatMovementEffectDisplayValue[]
        {
            new CombatMovementEffectDisplayValue("damage", ExtractDamage(combatMovementInstance, 0), CombatMovementEffectDisplayValueTemplate.Damage)
        };
    }
}