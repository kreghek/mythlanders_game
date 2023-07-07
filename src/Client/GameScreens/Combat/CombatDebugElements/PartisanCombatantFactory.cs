using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Partisan;

using Core.Combats;
using Core.Combats.CombatantStatus;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class PartisanCombatantFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<InspirationalBreakthroughFactory>(),

            CreateMovement<SabotageFactory>(),

            CreateMovement<SurpriseManeuverFactory>(),

            CreateMovement<BlankShotFactory>(),

            CreateMovement<OldGoodBrawlFactory>()
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
        stats.SetValue(ICombatantStatType.HitPoints, 4);
        stats.SetValue(ICombatantStatType.ShieldPoints, 3);
        stats.SetValue(ICombatantStatType.Resolve, 7);

        var hero = new Combatant("partisan", heroSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}