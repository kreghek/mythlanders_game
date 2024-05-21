using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class SpiralInversionFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage", ExtractDamage(combatMovementInstance, 1),
                DescriptionKeyValueTemplate.Damage),
            new DescriptionKeyValue("damage_resolve", ExtractDamage(combatMovementInstance, 3),
                DescriptionKeyValueTemplate.ResolveDamage)
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Melee;
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new AdjustPositionEffect(new SelfTargetSelector()),
                new DamageEffectWrapper(
                    new ClosestInLineTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(2)),
                new PushToPositionEffect(
                    new ClosestInLineTargetSelector(),
                    ChangePositionEffectDirection.ToVanguard),
                new ChangeCurrentStatEffect(
                    new ClosestInLineTargetSelector(),
                    CombatantStatTypes.Resolve,
                    GenericRange<int>.CreateMono(-2))
            });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}