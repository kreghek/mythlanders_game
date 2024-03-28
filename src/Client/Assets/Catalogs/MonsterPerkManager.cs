using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

internal sealed class MonsterPerkManager : IMonsterPerkManager
{
    private readonly MonsterPerkCatalog _catalog;
    private readonly GlobeProvider _globeProvider;
    private readonly IDice _dice;

    public MonsterPerkManager(IDice dice, MonsterPerkCatalog catalog, GlobeProvider globeProvider)
    {
        _dice = dice;
        _catalog = catalog;
        _globeProvider = globeProvider;
    }

    private MonsterPerk RollMonsterPerk()
    {
        var availableMonsterPerks = _catalog.Perks.ToArray();
        var filterUniquePerks = availableMonsterPerks.Except(_globeProvider.Globe.Player.MonsterPerks).ToArray();

        var monsterPerk = _dice.RollFromList(filterUniquePerks);

        return monsterPerk;
    }

    public IReadOnlyCollection<MonsterPerk> RollLocationPerks()
    {
        return new[] { RollMonsterPerk() };
    }
}