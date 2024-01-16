using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Partisan;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class PartisanHeroFactory : HeroFactoryBase
{
    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<BlankShotFactory>(),

            CreateMovement<InspirationalBreakthroughFactory>(),

            CreateMovement<EnergeticSuperiorityFactory>(),

            CreateMovement<SurpriseManeuverFactory>(),

            CreateMovement<OldGoodBrawlFactory>(),

            CreateMovement<SabotageFactory>()
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
        stats.SetValue(CombatantStatTypes.HitPoints, 4);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 3);
        stats.SetValue(CombatantStatTypes.Resolve, 7);

        return stats;
    }
}