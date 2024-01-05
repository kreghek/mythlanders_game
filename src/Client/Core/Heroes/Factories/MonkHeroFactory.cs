using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Monk;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal sealed class MonkHeroFactory : HeroFactoryBase
{
    protected override string ClassSid => "monk";

    protected override CombatMovementSequence CreateInitCombatMovementPool()
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

        return heroSequence;
    }

    protected override CombatantStatsConfig CreateInitStats()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 2);
        stats.SetValue(CombatantStatTypes.Resolve, 7);

        return stats;
    }
}