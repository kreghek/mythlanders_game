using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

public sealed class MonsterPerkManager : IMonsterPerkManager
{
    private readonly MonsterPerkCatalog _catalog;
    private readonly IDice _dice;

    public MonsterPerkManager(IDice dice, MonsterPerkCatalog catalog)
    {
        _dice = dice;
        _catalog = catalog;
    }

    private MonsterPerk RollMonsterPerk()
    {
        var availablePerkBuffs = _catalog.Perks.ToArray();

        var monsterPerk = _dice.RollFromList(availablePerkBuffs);

        return monsterPerk;
    }

    public IReadOnlyCollection<MonsterPerk> RollLocationPerks()
    {
        return new[] { RollMonsterPerk() };
    }
}