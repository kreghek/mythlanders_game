using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using GameAssets.Combats;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class EbonySkinFactory : SimpleCombatMovementFactoryBase
{
    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("defense", ExtractStatChangingValue(combatMovementInstance, 0),
                DescriptionKeyValueTemplate.ShieldPoints),
            new DescriptionKeyValue("defense_auto", 1,
                DescriptionKeyValueTemplate.ShieldPoints)
        };
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(
            new IEffect[]
            {
                new AddCombatantStatusEffect(
                    new SelfTargetSelector(),
                    new CombatStatusFactory(source =>
                    {
                        return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                            new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 3);
                    }))
            },
            new IEffect[]
            {
                new AddCombatantStatusEffect(
                    new SelfTargetSelector(),
                    new CombatStatusFactory(source =>
                    {
                        return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                            new ToEndOfCurrentRoundEffectLifetime(), source, CombatantStatTypes.Defense, 1);
                    }))
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.AutoDefense;
    }
}