using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Monster.Slavic.Chaser;

internal class GuardianPromiseFactory : SimpleCombatMovementFactoryBase
{
    protected override CombatMovementEffectConfig GetEffects()
    {
        return new CombatMovementEffectConfig(
                    new IEffect[]
                    {
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
                            UnitStatType.Defense,
                            3,
                            new ToNextCombatantTurnEffectLifetimeFactory())
                    },
                    new IEffect[]
                    {
                        new ChangeStatEffect(
                            new SelfTargetSelector(),
                            UnitStatType.Defense,
                            1,
                            new ToEndOfCurrentRoundEffectLifetimeFactory())
                    });
    }

    protected override CombatMovementTags GetTags() => CombatMovementTags.AutoDefense;
}
