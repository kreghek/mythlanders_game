using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.Effects;

using Core.Combats.TargetSelectors;

using GameAssets.Combats;

namespace Client.Assets.CombatMovements.Monster.Black.Aggressor;

internal class MadnessWithinEyesFactory : SimpleCombatMovementFactoryBase
{
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