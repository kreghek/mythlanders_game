using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Heroes;
using Client.Assets.Monsters;
using Client.Core;
using Client.GameScreens;

using Microsoft.Xna.Framework.Content;

namespace Client.Assets.Catalogs;

internal sealed class CombatantGraphicsCatalog : ICombatantGraphicsCatalog
{
    private readonly IDictionary<string, CombatantGraphicsConfigBase> _graphicsDict =
        new Dictionary<string, CombatantGraphicsConfigBase>();

    public CombatantGraphicsCatalog(GameObjectContentStorage gameObjectContentStorage)
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

    public void LoadContent(ContentManager contentManager)
    {
        foreach (var item in _graphicsDict)
        {
            item.Value.LoadContent(contentManager);
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

    public CombatantGraphicsConfigBase GetGraphics(string classSid)
    {
        return _graphicsDict[classSid.ToUpper()];
    }
}