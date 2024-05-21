using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;
using CombatDicesTeam.GenericRanges;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;
using GameAssets.Combats.AuraTargetSelectors;
using GameAssets.Combats.CombatantStatuses;
using GameAssets.Combats.CombatMovementEffects;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class DarkRaidsFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("damage", ExtractDamage(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.Damage),
            new DescriptionKeyValue("duration", 3, DescriptionKeyValueTemplate.TurnDuration),
            new DescriptionKeyValue("ranged_attack_debuff", 2, DescriptionKeyValueTemplate.DamageModifier)
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
        return new CombatMovementCost(2);
    }

    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return CombatMovementEffectConfig.Create(
            new IEffect[]
            {
                new DamageEffectWrapper(
                    new WeakestEnemyTargetSelector(),
                    DamageType.Normal,
                    GenericRange<int>.CreateMono(2)),
                new AddCombatantStatusEffect(
                    new SelfTargetSelector(),
                    new CombatStatusFactory(source => new AuraCombatantStatus(
                        new CombatantStatusSid(Sid),
                        new MultipleCombatantTurnEffectLifetime(3),
                        source,
                        combatant => new CombatStatusFactory(source2 =>
                            new ModifyDamageCalculatedCombatantStatus(
                                new CombatantStatusSid(Sid),
                                new OwnerBoundCombatantEffectLifetime(),
                                source2,
                                combatant1 => 2)),
                        new EnemyRearguardAuraTargetSelector())))
            });
    }

    /// <inheritdoc />
    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.Attack;
    }
}