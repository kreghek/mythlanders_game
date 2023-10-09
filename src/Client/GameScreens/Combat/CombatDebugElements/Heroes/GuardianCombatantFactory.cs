using System;
using System.Collections.Generic;

using Client.Assets.CombatMovements;
using Client.Assets.CombatMovements.Hero.Monk;
using Client.Assets.CombatMovements.Hero.Spearman;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.GameScreens.Combat.CombatDebugElements.Heroes;

public class GuardianCombatantFactory : IHeroCombatantFactory
{
    private static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<StonePathFactory>(),

            CreateMovement<PatientPredatorFactory>(),

            CreateMovement<DemonicTauntFactory>(),

            CreateMovement<PenetrationStrikeFactory>(),

            CreateMovement<DragonAngerFactory>()
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
        stats.SetValue(CombatantStatTypes.ShieldPoints, 5);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

        var hero = new TestamentCombatant("guardian", heroSequence, stats, combatActorBehaviour,
            ArraySegment<ICombatantStatusFactory>.Empty)
        {
            DebugSid = sid, IsPlayerControlled = true
        };
        return hero;
    }
}