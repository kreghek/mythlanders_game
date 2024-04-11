using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Hoplite;
using Client.Assets.GraphicConfigs.Heroes;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class HopliteHeroFactory : HeroFactoryBase
{
    public override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new HopliteGraphicsConfig(ClassSid);
    }

    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<PhalanxFactory>(),

            CreateMovement<OffensiveFactory>(),

            CreateMovement<ContemptFactory>(),

            CreateMovement<AresWarBringerThreadsFactory>()
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

    protected override IReadOnlyCollection<ICombatantStatusFactory> CreateStartupStatuses()
    {
        return new[]{
            new CombatStatusFactory(source => SystemStatuses.HasShield)

        };
    }
}