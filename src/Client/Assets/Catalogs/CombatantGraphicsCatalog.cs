using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Monsters;
using Client.Core;
using Client.Core.Heroes.Factories;
using Client.GameScreens;

using Microsoft.Xna.Framework.Content;

namespace Client.Assets.Catalogs;

internal sealed class CombatantGraphicsCatalog : ICombatantGraphicsCatalog
{
    private readonly IDictionary<string, CombatantGraphicsConfigBase> _graphicsDict =
        new Dictionary<string, CombatantGraphicsConfigBase>();

    public CombatantGraphicsCatalog(GameObjectContentStorage gameObjectContentStorage)
    {
        var heroes = CatalogHelper.GetAllFactories<IHeroFactory>();
        foreach (var factory in heroes)
        {
            var classSid = factory.ClassSid;
            var graphicsConfig = factory.GetGraphicsConfig();
            _graphicsDict.Add(classSid, graphicsConfig);
        }

        var monsters = LoadMonsterFactories();
        foreach (var factory in monsters)
        {
            var graphics = factory.CreateGraphicsConfig(gameObjectContentStorage);
            _graphicsDict.Add(factory.ClassName.ToString().ToLower(), graphics);
        }
    }

    private static IReadOnlyCollection<IMonsterFactory> LoadMonsterFactories()
    {
        var assembly = typeof(IMonsterFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(IMonsterFactory).IsAssignableFrom(x) && x != typeof(IMonsterFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<IMonsterFactory>().ToArray();
    }

    public void LoadContent(ContentManager contentManager)
    {
        foreach (var item in _graphicsDict)
        {
            item.Value.LoadContent(contentManager);
        }
    }

    public CombatantGraphicsConfigBase GetGraphics(string classSid)
    {
        return _graphicsDict[classSid];
    }
}