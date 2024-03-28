using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

internal sealed class MonsterPerkManager : IMonsterPerkManager
{
    private readonly MonsterPerkCatalog _catalog;
    private readonly Player _player;
    private readonly IDice _dice;

    public MonsterPerkManager(IDice dice, MonsterPerkCatalog catalog, Player player)
    {
        _dice = dice;
        _catalog = catalog;
        _player = player;
    }

    private MonsterPerk RollMonsterPerk()
    {
        var availableMonsterPerks = _catalog.Perks.ToArray();
        var filterUniquePerks = availableMonsterPerks.Except(_player.MonsterPerks).ToArray();

        var monsterPerk = _dice.RollFromList(filterUniquePerks);

        return monsterPerk;
    }

    public IReadOnlyCollection<MonsterPerk> RollLocationPerks()
    {
        return new[] { RollMonsterPerk() };
    }
}