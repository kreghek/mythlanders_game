using System.Collections.Generic;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

public sealed class MonsterPerkManager : IMonsterPerkManager
{
    private readonly IDice _dice;

    public MonsterPerkManager(IDice dice)
    {
        _dice = dice;
    }

    public IReadOnlyCollection<MonsterPerk> RollLocationPerks()
    {
        return new[] { RollMonsterPerk() };
    }
    
    private MonsterPerk RollMonsterPerk()
    {
        var availablePerkBuffs = new[]
        {
            MonsterPerkCatalog.ExtraHp,
            MonsterPerkCatalog.ExtraSp,
            MonsterPerkCatalog.ImprovedAllDamage,
            MonsterPerkCatalog.ImprovedMeleeDamage
        };

        var monsterPerk = _dice.RollFromList(availablePerkBuffs);

        return monsterPerk;
    }
}