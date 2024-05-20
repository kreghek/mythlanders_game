using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.MonsterPerks;
using Client.Core;

using CombatDicesTeam.Dices;

namespace Client.Assets.Catalogs;

internal sealed class MonsterPerkManager : IMonsterPerkManager
{
    private readonly IMonsterPerkCatalog _catalog;
    private readonly IDice _dice;
    private readonly GlobeProvider _globeProvider;

    public MonsterPerkManager(IDice dice, IMonsterPerkCatalog catalog, GlobeProvider globeProvider)
    {
        _dice = dice;
        _catalog = catalog;
        _globeProvider = globeProvider;
    }

    private static IReadOnlyCollection<MonsterPerk> RollPerks(MonsterCombatantPrefab monsterCombatantPrefab,
        IEnumerable<MonsterPerk> playerMonsterPerks,
        IDice dice)
    {
        var filteredPerks = playerMonsterPerks
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

    private MonsterPerk RollRewardMonsterPerk()
    {
        var availableMonsterPerks = _catalog.Perks.Where(x => !x.CantBeRolledAsReward).ToArray();
        var filterUniquePerks = availableMonsterPerks
            .Except(_globeProvider.Globe.Player.MonsterPerks.Where(x => x.IsUnique)).ToArray();

        var monsterPerk = _dice.RollFromList(filterUniquePerks);

        return monsterPerk;
    }

    public IReadOnlyCollection<MonsterPerk> RollLocationRewardPerks()
    {
        if (!_globeProvider.Globe.Features.HasFeature(GameFeatures.MonsterPerksCollecting))
        {
            return ArraySegment<MonsterPerk>.Empty;
        }

        return new[] { RollRewardMonsterPerk() };
    }

    public IReadOnlyCollection<MonsterPerk> RollMonsterPerks(MonsterCombatantPrefab targetMonsterPrefab)
    {
        if (!_globeProvider.Globe.Features.HasFeature(GameFeatures.UseMonsterPerks))
        {
            return ArraySegment<MonsterPerk>.Empty;
        }

        return RollPerks(targetMonsterPrefab, _globeProvider.Globe.Player.MonsterPerks, _dice);
    }
}