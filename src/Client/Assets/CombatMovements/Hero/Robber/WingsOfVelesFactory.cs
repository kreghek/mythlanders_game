using Core.Combats;
using Core.Combats.CombatantEffects;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.Assets.CombatMovements.Hero.Robber;

internal class WingsOfVelesFactory : CombatMovementFactoryBase
{
    /// <inheritdoc />
    public override CombatMovementIcon CombatMovementIcon => new(1, 7);

    /// <inheritdoc />
    public override CombatMovement CreateMovement()
    {
        var combatantEffectFactory = new ModifyCombatantMoveStatsCombatantEffectFactory(
            new CombatantEffectSid(Sid),
            new MultipleCombatantTurnEffectLifetimeFactory(1),
            CombatantMoveStats.Cost,
            -1);

        return new CombatMovement(Sid,
            new CombatMovementCost(1),
            CombatMovementEffectConfig.Create(
                new IEffect[]
                {
                    new ModifyEffectsEffect(new CombatantEffectSid(Sid), new SelfTargetSelector(), 1),
                    new AddCombatantEffectEffect(new SelfTargetSelector(), combatantEffectFactory)
                })
        );
    }
}