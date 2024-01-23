using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Client.Assets.CombatMovements;
using Client.Assets.GraphicConfigs;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;

namespace Client.Core.Heroes.Factories;

internal abstract class HeroFactoryBase : IHeroFactory
{
    public virtual string ClassSid => GetType().Name[..^"HeroFactory".Length];
    protected abstract CombatMovementSequence CreateInitCombatMovementPool();

    protected abstract CombatantStatsConfig CreateInitStats();

    protected static CombatMovement CreateMovement<T>() where T : ICombatMovementFactory
    {
        return Activator.CreateInstance<T>().CreateMovement();
    }

    protected virtual IReadOnlyCollection<ICombatantStatusFactory> CreateStartupStatuses()
    {
        return Array.Empty<ICombatantStatusFactory>();
    }

    public HeroState Create()
    {
        var heroSequence = CreateInitCombatMovementPool();
        var stats = CreateInitStats();
        var startupStatuses = CreateStartupStatuses();

        var hp = stats.GetStats().Single(x => ReferenceEquals(x.Type, CombatantStatTypes.HitPoints)).Value;
        var combatantStats = stats.GetStats().Where(x => !ReferenceEquals(x.Type, CombatantStatTypes.HitPoints)).ToArray();

        var hero = new HeroState(ClassSid, hp, combatantStats, heroSequence.Items, startupStatuses);
        return hero;
    }
    
    public virtual CombatantGraphicsConfigBase GetGraphicsConfig()
    {
        return new SingleSpriteGraphicsConfig(Path.Combine(CommonConstants.PathToCharacterSprites, "Heroes",
            ClassSid, "Thumbnail"));
    }
}