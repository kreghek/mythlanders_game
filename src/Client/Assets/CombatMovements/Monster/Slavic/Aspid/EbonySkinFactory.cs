using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.Aspid;

internal class EbonySkinFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(
            new IEffect[]
            {
                new ChangeStatEffect(
                    new CombatantEffectSid(Sid),
                    new SelfTargetSelector(),
                    ICombatantStatType.Defense,
                    3,
                    new ToNextCombatantTurnEffectLifetimeFactory())
            },
            new IEffect[]
            {
                new ChangeStatEffect(
                    new CombatantEffectSid(Sid),
                    new SelfTargetSelector(),
                    ICombatantStatType.Defense,
                    1,
                    new ToEndOfCurrentRoundEffectLifetimeFactory())
            });
    }

    protected override CombatMovementTags GetTags()
    {
        return CombatMovementTags.AutoDefense;
    }
}