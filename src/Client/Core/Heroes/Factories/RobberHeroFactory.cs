using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Robber;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;

using Core.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal sealed class RobberHeroFactory : HeroFactoryBase
{
    protected override string ClassSid => "robber";

    public TestamentCombatant Create(string sid, ICombatActorBehaviour combatActorBehaviour, IStatValue hitpointsStat)
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<ArrowsOfMoranaFactory>(),

            CreateMovement<BalticThunderFactory>(),

            CreateMovement<UndercutValuesFactory>(),

            CreateMovement<WingsOfVelesFactory>(),

            CreateMovement<WindWheelFactory>()
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
                new OwnerBoundCombatantStatusLifetimeFactory())
        };

        var hero = new TestamentCombatant("robber", heroSequence, stats, combatActorBehaviour, startupEffects)
        {
            DebugSid = sid, IsPlayerControlled = true
        };

        return hero;
    }

    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<ArrowsOfMoranaFactory>(),

            CreateMovement<BalticThunderFactory>(),

            CreateMovement<UndercutValuesFactory>(),

            CreateMovement<WingsOfVelesFactory>(),

            CreateMovement<WindWheelFactory>()
        };

        var heroSequence = new CombatMovementSequence();

        for (var i = 0; i < 2; i++)
        {
            foreach (var movement in movementPool)
            {
                heroSequence.Items.Add(movement);
            }
        }

        return heroSequence;
    }

    protected override CombatantStatsConfig CreateInitStats()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 3);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 0);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

        return stats;
    }
}