using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Assets.Heroes;
using Rpg.Client.Assets.Monsters;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Catalogs
{
    internal sealed class DemoUnitSchemeCatalog : IUnitSchemeCatalog
    {
        public DemoUnitSchemeCatalog(IBalanceTable balanceTable, bool isDemo)
        {
            var heroes = LoadHeroFactories().Where(x => !isDemo || (x.IsReleaseReady && isDemo)).ToArray();

            Heroes = heroes.Select(x => x.Create(balanceTable)).ToDictionary(scheme => scheme.Name, scheme => scheme);

            var monsterFactories = LoadMonsterFactories();
            var loadedMonsters = monsterFactories.Select(x => x.Create(balanceTable));

            AllMonsters = loadedMonsters.ToArray();
        }

        private static IReadOnlyCollection<IHeroFactory> LoadHeroFactories()
        {
            var assembly = typeof(IHeroFactory).Assembly;
            var factoryTypes = assembly.GetTypes()
                .Where(x => typeof(IHeroFactory).IsAssignableFrom(x) && x != typeof(IHeroFactory));
            var factories = factoryTypes.Select(Activator.CreateInstance);
            return factories.OfType<IHeroFactory>().ToArray();
        }
        
        private static IReadOnlyCollection<IMonsterFactory> LoadMonsterFactories()
        {
            var assembly = typeof(IMonsterFactory).Assembly;
            var factoryTypes = assembly.GetTypes()
                .Where(x => typeof(IMonsterFactory).IsAssignableFrom(x) && x != typeof(IMonsterFactory));
            var factories = factoryTypes.Select(Activator.CreateInstance);
            return factories.OfType<IMonsterFactory>().ToArray();
        }

        public IDictionary<UnitName, UnitScheme> Heroes { get; }

        public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    }
}