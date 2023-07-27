using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Robber;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

using Core.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class RobberCombatantFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<BalticThunderFactory>(),

            CreateMovement<ArrowsOfMoranaFactory>(),

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
        stats.SetValue(CombatantStatTypes.HitPoints, hitpointsStat);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 0);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

        var startupEffects = new[]
        {
            new ImpulseGeneratorCombatantStatusFactory(
                CombatantStatusSids.ImpulseGenerator,
                CombatantStatusSids.Impulse,
                new CombatantActiveCombatantEffectLifetimeFactory())
        };

        var hero = new TestamentCombatant("robber", heroSequence, stats, combatActorBehaviour, startupEffects)
        {
            DebugSid = sid, IsPlayerControlled = true
        };

        return hero;
    }
}