using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Monk;
using Client.Assets.CombatMovements.Hero.Partisan;

using Core.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class MonkFactory : IHeroCombatantFactory
{
    public Combatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<HandOfThousandFormsFactory>(),

            CreateMovement<NinthTrigramFactory>(),

            CreateMovement<ArtOfCombatFactory>(),

            CreateMovement<MasterfulStaffHitFactory>(),

            CreateMovement<HiddenIntentionFactory>()
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
        stats.SetValue(UnitStatType.ShieldPoints, 3);
        stats.SetValue(UnitStatType.Resolve, 7);

        var hero = new Combatant("monk", heroSequence, stats, combatActorBehaviour)
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