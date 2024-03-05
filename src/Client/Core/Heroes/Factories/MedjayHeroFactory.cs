using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Medjay;
using Client.Assets.CombatMovements.Hero.Partisan;
using Client.Assets.GraphicConfigs.Heroes;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class MedjayHeroFactory : HeroFactoryBase
{
    public override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new MedjayGraphicsConfig();
    }

    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<ChorusEyeFactory>(),

            CreateMovement<PoisonCutFactory>(),

            CreateMovement<SunburstFactory>(),

            CreateMovement<SurpriseManeuverFactory>(),

            CreateMovement<SuperNaturalAgilityFactory>()
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
        stats.SetValue(CombatantStatTypes.ShieldPoints, 1);
        stats.SetValue(CombatantStatTypes.Resolve, 7);

        return stats;
    }
}