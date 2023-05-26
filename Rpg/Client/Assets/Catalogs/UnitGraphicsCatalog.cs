using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Heroes;
using Client.Assets.Monsters;
using Client.GameScreens;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs;

internal sealed class UnitGraphicsCatalog : IUnitGraphicsCatalog
{
    private readonly IDictionary<string, UnitGraphicsConfigBase> _graphicsDict =
        new Dictionary<string, UnitGraphicsConfigBase>();

    public UnitGraphicsCatalog(GameObjectContentStorage gameObjectContentStorage)
    {
        var heroes = LoadHeroFactories();
        foreach (var factory in heroes)
        {
            var graphics = factory.CreateGraphicsConfig(gameObjectContentStorage);
            _graphicsDict.Add(factory.HeroName.ToString().ToUpper(), graphics);
        }

        var monsters = LoadMonsterFactories();
        foreach (var factory in monsters)
        {
            var graphics = factory.CreateGraphicsConfig(gameObjectContentStorage);
            _graphicsDict.Add(factory.ClassName.ToString().ToUpper(), graphics);
        }
    }

    private static IReadOnlyCollection<IHeroFactory> LoadHeroFactories()
    {
        var assembly = typeof(IHeroFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(IHeroFactory).IsAssignableFrom(x) && x != typeof(IHeroFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<IHeroFactory>().ToArray();
    }

    private static IReadOnlyCollection<IMonsterFactory> LoadMonsterFactories()
    {
        var assembly = typeof(IMonsterFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(IMonsterFactory).IsAssignableFrom(x) && x != typeof(IMonsterFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<IMonsterFactory>().ToArray();
    }

    public UnitGraphicsConfigBase GetGraphics(string classSid)
    {
        return _graphicsDict[classSid.ToUpper()];
    }
}