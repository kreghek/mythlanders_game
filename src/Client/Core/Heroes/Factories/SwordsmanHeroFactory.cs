using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Swordsman;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal sealed class SwordsmanHeroFactory : HeroFactoryBase
{
    protected override string ClassSid => "swordsman";

    protected override CombatMovementSequence CreateInitCombatMovementPool()
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

        return heroSequence;
    }

    protected override CombatantStatsConfig CreateInitStats()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 4);
        stats.SetValue(CombatantStatTypes.Resolve, 5);

        return stats;
    }
}