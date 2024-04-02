﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Combats.CombatantStatuses;
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

    private MonsterPerk RollRewardMonsterPerk()
    {
        var availableMonsterPerks = _catalog.Perks.ToArray();
        var filterUniquePerks = availableMonsterPerks.Except(_globeProvider.Globe.Player.MonsterPerks).ToArray();

        var monsterPerk = _dice.RollFromList(filterUniquePerks);

        return monsterPerk;
    }

    public IReadOnlyCollection<MonsterPerk> RollLocationRewardPerks()
    {
        return new[] { RollRewardMonsterPerk() };
    }

    public IReadOnlyCollection<MonsterPerk> RollMonsterPerks(MonsterCombatantPrefab targetMonsterPrefab)
    {
        return RollPerks(targetMonsterPrefab, _globeProvider.Globe.Player.MonsterPerks, _dice);
    }
    
    private static IReadOnlyCollection<MonsterPerk> RollPerks(MonsterCombatantPrefab monsterCombatantPrefab,
        IReadOnlyCollection<MonsterPerk> availableMonsterPerks,
        IDice dice)
    {
        var filteredPerks = availableMonsterPerks
            .Where(x => x.Predicates.All(p => p.IsApplicableTo(monsterCombatantPrefab))).ToArray();
        
        var count = dice.Roll(0, filteredPerks.Length);

        switch (count)
        {
            case < 0:
                throw new InvalidOperationException("Rolled perk count can't be below zero.");
            case 0:
                return ArraySegment<MonsterPerk>.Empty;
            default:
                {
                    var monsterPerk = dice.RollFromList(filteredPerks.ToArray(), count).ToArray();
        
                    return monsterPerk.ToArray();
                }
        }
    }
}