using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Sage;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class SageHeroFactory : HeroFactoryBase
{
    protected override string ClassSid => "sage";

    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<AskedNoViolenceFactory>(),

            CreateMovement<FaithBoostFactory>(),

            CreateMovement<NoViolencePleaseFactory>(),

            CreateMovement<ReproachFactory>()
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
        stats.SetValue(CombatantStatTypes.Resolve, 3);
        return stats;
    }
}