using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Partisan;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class PartisanFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
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

        var hero = new Combatant("partisan", heroSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = true
        };
        return hero;
    }
    
    private static CombatMovement CreateMovement<T>() where T: ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }
}