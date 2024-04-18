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
        var heroes = CatalogHelper.GetAllFactories<IHeroFactory>().ToArray();

        AvailableHeroes = heroes.Select(x => x.ClassSid).ToArray();

        var monsterFactories = CatalogHelper.GetAllFactories<IMonsterFactory>();
        var loadedMonsters = monsterFactories.Select(x => x.Create(balanceTable));

        AllMonsters = loadedMonsters.ToArray();
    }

    public IReadOnlyCollection<UnitScheme> AllMonsters { get; }
    public IReadOnlyCollection<string> AvailableHeroes { get; }
}