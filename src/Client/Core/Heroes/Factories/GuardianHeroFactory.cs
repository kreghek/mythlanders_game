using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Spearman;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal sealed class GuardianHeroFactory : HeroFactoryBase
{
    protected override string ClassSid => "guardian";

    protected override CombatMovementSequence CreateInitCombatMovementPool()
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

        return heroSequence;
    }

    protected override CombatantStatsConfig CreateInitStats()
    {
        var stats = new CombatantStatsConfig();
        stats.SetValue(CombatantStatTypes.HitPoints, 5);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 5);
        stats.SetValue(CombatantStatTypes.Resolve, 4);

        return stats;
    }
}