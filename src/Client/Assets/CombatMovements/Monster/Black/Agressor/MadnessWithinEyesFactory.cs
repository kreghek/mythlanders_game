using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Black.Agressor;

internal class MadnessWithinEyesFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(
            new IEffect[]
            {
                new ChangeStatEffect(
                    new CombatantEffectSid(Sid),
                    new SelfTargetSelector(),
                    CombatantStatTypes.Defense,
                    3,
                    new ToNextCombatantTurnEffectLifetimeFactory())
            },
            new IEffect[]
            {
                new ChangeStatEffect(
                    new CombatantEffectSid(Sid),
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