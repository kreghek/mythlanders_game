using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Combats.Effects;

using GameAssets.Combats;

using SelfTargetSelector = Core.Combats.TargetSelectors.SelfTargetSelector;

namespace Client.Assets.CombatMovements.Monster.Slavic.CorruptedBear;

internal sealed class CoverOfDewFactory : SimpleCombatMovementFactoryBase
{
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
}