using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Swordsman;

using Core.Combats;
using Core.Combats.CombatantEffects;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class SwordsmanCombatantFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<RiseYourSwordsFactory>(),

            CreateMovement<DieBySwordFactory>(),

            CreateMovement<StayStrongFactory>(),

            CreateMovement<HitFromShoulderFactory>(),

            CreateMovement<LookOutFactory>()
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
        stats.SetValue(UnitStatType.HitPoints, hitpointsStat);
        stats.SetValue(UnitStatType.ShieldPoints, 4);
        stats.SetValue(UnitStatType.Resolve, 5);

        var hero = new Combatant("swordsman", heroSequence, stats, combatActorBehaviour, ArraySegment<ICombatantEffectFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}