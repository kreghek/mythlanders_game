using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Partisan;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class PartisanFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>();

        movementPool.Add(CreateMovement<InspirationalBreakthroughFactory>());

        movementPool.Add(CreateMovement<SabotageFactory>());

        movementPool.Add(CreateMovement<SurpriseManeuverFactory>());

        movementPool.Add(CreateMovement<BlankShotFactory>());

        movementPool.Add(CreateMovement<OldGoodBrawlFactory>());

        var heroSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            foreach (var movement in movementPool)
            {
                heroSequence.Items.Add(movement);
            }
        }

        var stats = new CombatantStatsConfig();
        stats.SetValue(UnitStatType.HitPoints, 4);
        stats.SetValue(UnitStatType.ShieldPoints, 3);
        stats.SetValue(UnitStatType.Resolve, 7);

        var hero = new Combatant("partisan", heroSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}