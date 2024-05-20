using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.CombatMovementEffects;

using JetBrains.Annotations;

namespace Client.Assets.CombatMovements.Hero.Partisan;

[UsedImplicitly]
internal class OldGoodBrawlFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(4, 4);

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage", ExtractDamage(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.Damage)
        };
    }

    /// <inheritdoc />
    protected override IEnumerable<CombatMovementMetadataTrait> CreateTraits()
    {
        yield return CombatMovementMetadataTraits.Melee;
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(new IEffect[]
        {
            new DamageEffectWrapper(
                new ClosestInLineTargetSelector(),
                DamageType.Normal,
                GenericRange<int>.CreateMono(2)),
            new PushToPositionEffect(
                new SelfTargetSelector(),
                ChangePositionEffectDirection.ToVanguard
            )
        });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(2);
    }
}