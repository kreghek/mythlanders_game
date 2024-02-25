using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Priest;
using Client.Assets.GraphicConfigs.Heroes;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class PriestHeroFactory : HeroFactoryBase
{
    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<DarkLightningFactory>(),

            CreateMovement<UnlimitedSinFactory>(),

            CreateMovement<ParalyticChoirFactory>(),

            CreateMovement<FingerOfAnubisFactory>()
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
        stats.SetValue(CombatantStatTypes.HitPoints, 2);
        stats.SetValue(CombatantStatTypes.ShieldPoints, 0);
        stats.SetValue(CombatantStatTypes.Resolve, 6);
        return stats;
    }

    public override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new PriestGraphicsConfig();
    }
}