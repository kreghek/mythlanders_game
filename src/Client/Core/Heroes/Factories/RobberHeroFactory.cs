using System.Collections.Generic;

using Client.Assets.CombatMovements.Hero.Robber;
using Client.Assets.GraphicConfigs.Heroes;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

using JetBrains.Annotations;

namespace Client.Core.Heroes.Factories;

[UsedImplicitly]
internal sealed class RobberHeroFactory : HeroFactoryBase
{
    public override CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new RobberGraphicsConfig(ClassSid);
    }

    protected override CombatMovementSequence CreateInitCombatMovementPool()
    {
        var movementPool = new List<CombatMovement>
        {
            CreateMovement<ArrowsOfMoranaFactory>(),

            CreateMovement<BalticThunderFactory>(),

            CreateMovement<WeOnlyLiveOnceFactory>(),

            CreateMovement<WingsOfVelesFactory>(),

            CreateMovement<WindWheelFactory>()
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

    protected override IReadOnlyCollection<ICombatantStatusFactory> CreateStartupStatuses()
    {
        var startupStatuses = new[]
        {
            new CombatStatusFactory(source => new ImpulseGeneratorCombatantStatus(
                CombatantStatusSids.ImpulseGenerator,
                CombatantStatusSids.Impulse,
                new OwnerBoundCombatantEffectLifetime(),
                source))
        };

        return startupStatuses;
    }
}