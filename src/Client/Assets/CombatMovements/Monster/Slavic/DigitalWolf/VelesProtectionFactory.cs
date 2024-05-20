using System.Collections.Generic;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.Effects;

using GameAssets.Combats;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Monster.Slavic.DigitalWolf;

internal class VelesProtectionFactory : CombatMovementFactoryBase
{
    public override CombatMovement CreateMovement()
    {
        return new CombatMovement(Sid,
            new CombatMovementCost(0),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new AddCombatantStatusEffect(
                        new SelfTargetSelector(),
                        new CombatStatusFactory(source =>
                        {
                            return new ModifyStatCombatantStatus(new CombatantStatusSid(Sid),
                                new ToNextCombatantTurnEffectLifetime(), source, CombatantStatTypes.ShieldPoints, 2);
                        })),
                    new PushToPositionEffect(new SelfTargetSelector(), ChangePositionEffectDirection.ToRearguard)
                })
        );
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
}