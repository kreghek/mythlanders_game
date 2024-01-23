using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.ShieldBearer;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class ShieldBearerHeroFactory : HeroFactoryBase
{
    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<BlindDefenseFactory>(),
            CreateMovement<SuppressiveFireFactory>()
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
        stats.SetValue(CombatantStatTypes.ShieldPoints, 5);
        stats.SetValue(CombatantStatTypes.Resolve, 7);
        return stats;
    }
}