using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Amazon;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal sealed class AmazonHeroFactory : HeroFactoryBase
{
    protected override string ClassSid => "amazon";

    protected override CombatMovementSequence CreateInitCombatMovementPool()
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