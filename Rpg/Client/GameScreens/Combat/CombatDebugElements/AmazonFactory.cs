using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Amazon;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class AmazonFactory : IHeroCombatantFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<HuntFactory>(),

            CreateMovement<FinishWoundedFactory>(),

            CreateMovement<TrackerSavvyFactory>(),

            CreateMovement<JustHitBoarWithKnifeFactory>(),

            CreateMovement<BringBeastDownFactory>()
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
        stats.SetValue(UnitStatType.HitPoints, 3);
        stats.SetValue(UnitStatType.ShieldPoints, 0);
        stats.SetValue(UnitStatType.Resolve, 4);

        var hero = new Combatant("amazon", heroSequence, stats, combatActorBehaviour)
        {
            Sid = sid, IsPlayerControlled = true
        };
        return hero;
    }

    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }
}