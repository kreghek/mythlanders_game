using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Monk;

using Core.Combats;
using Core.Combats.CombatantStatuses;

namespace Client.GameScreens.Combat.CombatDebugElements;

public class MonkCombatantFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

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
        stats.SetValue(CombatantStatTypes.HitPoints, hitpointsStat);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 3);
        stats.SetValue(CombatantStatTypes.Resolve, 7);

        var hero = new Combatant("monk", heroSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}