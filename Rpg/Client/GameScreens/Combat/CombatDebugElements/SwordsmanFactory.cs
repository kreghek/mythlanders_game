using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Swordsman;

using Core.Combats;
using Core.Combats.CombatantEffectLifetimes;
using Core.Combats.Effects;
using Core.Combats.TargetSelectors;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class SwordsmanFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour)
    {
        var movementPool = new List<CombatMovement>();

        movementPool.Add(CreateMovement<RiseYourSwordsFactory>());

        movementPool.Add(CreateMovement<DieBySwordFactory>());

        movementPool.Add(CreateMovement<StayStrongFactory>());

        movementPool.Add(CreateMovement<HitFromShoulderFactory>());

        movementPool.Add(CreateMovement<LookOutFactory>());

        var heroSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            foreach (var movement in movementPool)
            {
                heroSequence.Items.Add(movement);
            }
        }
        
        var stats = new CombatantStatsConfig();

        var hero = new Combatant("swordsman", heroSequence, stats, combatActorBehaviour)
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