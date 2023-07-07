using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Robber;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.CombatantEffects;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class RobberCombatantFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<BalticThunderFactory>(), // hunt

            CreateMovement<ArrowOfMoranaFactory>(), // finish

            CreateMovement<WingsOfVelesFactory>(), // tracker

            CreateMovement<UndercutValuesFactory>(), // just

            CreateMovement<WindWheelFactory>() // bring
        };

        var heroSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            foreach (var movement in movementPool)
            {
                heroSequence.Items.Add(movement);
            }
        }

        var stats = new CombatantStatsConfig();
        stats.SetValue(ICombatantStatType.HitPoints, hitpointsStat);
        stats.SetValue(ICombatantStatType.ShieldPoints, 0);
        stats.SetValue(ICombatantStatType.Resolve, 4);

        var startupEffects = new[]
        {
            new ImpulseGeneratorCombatantEffectFactory(
                CombatantEffectSids.ImpulseGenerator,
                CombatantEffectSids.Impulse,
                new CombatantActiveCombatantEffectLifetimeFactory())
        };

        var hero = new Combatant("robber", heroSequence, stats, combatActorBehaviour, startupEffects)
        {
            DebugSid = sid, IsPlayerControlled = true
        };

        return hero;
    }
}