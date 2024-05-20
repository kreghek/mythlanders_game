using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;

using GameAssets.Combats;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class VelesProtectionFactory : SimpleCombatMovementFactoryBase
{
    /// <inheritdoc />
    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new SelfTargetSelector(),
                        new CombatStatusFactory(source =>
                        {
                            return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                                new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.ShieldPoints, 2);
                        })),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                },
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new SelfTargetSelector(),
                        new CombatStatusFactory(source =>
                        {
                            return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                                new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.Defense, 2);
                        })),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                });
    }

    public override IReadOnlyList<DescriptionKeyValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new DescriptionKeyValue("sp", ExtractStatChangingValue(combatMovementInstance, 1),
                DescriptionKeyValueTemplate.ShieldPoints)
        };
    }

    /// <inheritdoc />
    protected override CombatMovementCost GetCost()
    {
        return new CombatMovementCost(1);
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.AutoDefense;
    }
}