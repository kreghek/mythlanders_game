using System.Collections.Generic;
using System.Linq;

using Client.Core;

namespace Client.Assets.MonsterPerks;

public class MonsterPerkCatalog : IMonsterPerkCatalog
{
    public MonsterPerkCatalog()
    {
        var factories = CatalogHelper.GetAllFactories<IMonsterPerkFactory>(typeof(IMonsterPerkFactory).Assembly);
        Perks = factories.Select(x => x.Create()).ToArray();
    }

    public IReadOnlyCollection<MonsterPerk> Perks { get; }
}