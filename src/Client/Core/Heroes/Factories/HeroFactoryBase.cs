using System;
using System.Linq;

using Client.Assets.CombatMovements;

using CombatDicesTeam.Combats;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal abstract class HeroFactoryBase : IHeroFactory
{
    protected abstract string ClassSid { get; }
    protected abstract CombatMovementSequence CreateInitCombatMovementPool();

    protected abstract CombatantStatsConfig CreateInitStats();

    public HeroState Create()
    {
        var heroSequence = CreateInitCombatMovementPool();
        var stats = CreateInitStats();

        var hero = new HeroState(ClassSid, stats.GetStats().Single(x => x.Type == CombatantStatTypes.HitPoints).Value,
            stats.GetStats().Where(x => x.Type != CombatantStatTypes.HitPoints).ToArray(), heroSequence.Items);
        return hero;
    }

    protected static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }
}
