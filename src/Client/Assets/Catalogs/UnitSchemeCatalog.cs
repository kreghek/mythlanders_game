using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Monsters;
using Client.Core;
using Client.Core.Heroes.Factories;

namespace Client.Assets.Catalogs;

internal sealed class UnitSchemeCatalog : ICharacterCatalog
{
    public UnitSchemeCatalog(IBalanceTable balanceTable)
    {
        var heroes = LoadHeroFactories().ToArray();

        AvailableHeroes = heroes.Select(x => x.ClassSid).ToArray();
        
        var monsterFactories = LoadMonsterFactories();
        var loadedMonsters = monsterFactories.Select(x => x.Create(balanceTable));

        AllMonsters = loadedMonsters.ToArray();
    }

    private static IEnumerable<IMonsterFactory> LoadMonsterFactories()
    {
        var assembly = typeof(IMonsterFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(IMonsterFactory).IsAssignableFrom(x) && x != typeof(IMonsterFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<IMonsterFactory>().ToArray();
    }
    
    private static IEnumerable<IHeroFactory> LoadHeroFactories()
    {
        var assembly = typeof(IHeroFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(IHeroFactory).IsAssignableFrom(x) && x != typeof(IHeroFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<IHeroFactory>().ToArray();
    }


    public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    public IReadOnlyCollection<string> AvailableHeroes { get; }
}