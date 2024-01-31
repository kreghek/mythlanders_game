using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class EbonySkinFactory : SimpleCombatMovementFactoryBase
{
    public override IReadOnlyList<CombatMovementEffectDisplayValue> ExtractEffectsValues(
        CombatMovementInstance combatMovementInstance)
    {
        return new[]
        {
            new CombatMovementEffectDisplayValue("defense", ExtractStatChangingValue(combatMovementInstance, 0),
                CombatMovementEffectDisplayValueTemplate.ShieldPoints),
            new CombatMovementEffectDisplayValue("defense_auto", ExtractStatChangingValue(combatMovementInstance, 1),
                CombatMovementEffectDisplayValueTemplate.ShieldPoints)
        };
    }

    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(
            new IEffect[]
            {
                new ChangeStatEffect(
                    new CombatantStatusSid(Sid),
                    new SelfTargetSelector(),
                    CombatantStatTypes.Defense,
                    3,
                    new ToNextCombatantTurnEffectLifetimeFactory())
            },
            new IEffect[]
            {
                new ChangeStatEffect(
                    new CombatantStatusSid(Sid),
                    new SelfTargetSelector(),
                    CombatantStatTypes.Defense,
                    1,
                    new ToEndOfCurrentRoundEffectLifetimeFactory())
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.AutoDefense;
    }
}