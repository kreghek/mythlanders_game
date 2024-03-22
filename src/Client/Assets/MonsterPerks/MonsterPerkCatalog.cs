using System.Collections.Generic;
using System.Linq;

using Client.Core;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

namespace Client.Assets.MonsterPerks;

public class MonsterPerkCatalog
{
    public IReadOnlyCollection<MonsterPerk> Perks { get; }

    public void Init()
    {
        var factories = CatalogHelper.GetAllFromStaticCatalog<IMonsterPerkFactory>(typeof(IMonsterPerkFactory).Assembly);
        Perks = factories.Select(x => x.Create()).ToArray();
    }
}