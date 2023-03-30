using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Heroes;

using Core.Combats;
using Core.Dices;
using Core.PropDrop;

using Rpg.Client.Assets.Perks;
using Rpg.Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class CombatCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly LocationSid _locationSid;
    private readonly CampaignStageTemplateServices _services;

    public CombatCampaignStageTemplateFactory(LocationSid locationSid, CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _services = services;
    }

    private IReadOnlyList<(UnitName name, int level)> GetStartMonsterInfoList(LocationSid location)
    {
        var availableAllRegularMonsters =
            _services.UnitSchemeCatalog.AllMonsters.Where(x => !HasPerk<BossMonster>(x, 1));

        var filteredByLocationMonsters = availableAllRegularMonsters.Where(x =>
            x.LocationSids is null || x.LocationSids is not null && x.LocationSids.Contains(location));

        var availableMonsters = filteredByLocationMonsters.ToList();

        var rolledUnits = new List<UnitScheme>();

        for (var i = 0; i < 3; i++)
        {
            if (!availableMonsters.Any())
            {
                break;
            }

            var scheme = _services.Dice.RollFromList(availableMonsters, 1).Single();

            rolledUnits.Add(scheme);

            if (scheme.IsUnique)
            {
                // Remove all unique monsters from roll list.
                availableMonsters.RemoveAll(x => x.IsUnique);
            }
        }

        return rolledUnits.Select(x => (x.Name, 2)).ToArray();
    }

    private static bool HasPerk<TPerk>(UnitScheme unitScheme, int combatLevel)
    {
        var unit = new Hero(unitScheme, combatLevel);
        return unit.Perks.OfType<TPerk>().Any();
    }

    /// <inheritdoc />
    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var monsterCombatantPrefabs = new[]
        {
            new MonsterCombatantPrefab("chaser", 0, new FieldCoords(0, 1)),
            //new MonsterCombatantPrefab("chaser", 1, new FieldCoords(1, 2)),
            //new MonsterCombatantPrefab("digitalwolf", 0, new FieldCoords(0, 2)),
        };

        var monsterResources = GetMonsterDropTables(monsterCombatantPrefabs);

        var totalDropTables = new List<IDropTableScheme>();
        totalDropTables.AddRange(monsterResources);
        totalDropTables.Add(new DropTableScheme("combat-xp", new IDropTableRecordSubScheme[]
        {
            new DropTableRecordSubScheme(null, new Range<int>(1, 2), "combat-xp", 1)
        }, 1));

        var combat = new CombatSource(
            monsterCombatantPrefabs,
            new CombatReward(totalDropTables.ToArray())
            );

        var combatSequence = new CombatSequence
        {
            Combats = new[] { combat }
        };

        var location = new GlobeNode
        {
            Sid = _locationSid,
            AssignedCombats = combatSequence
        };
        var stageItem = new CombatStageItem(location, combatSequence);

        var monsterInfos = GetStartMonsterInfoList(_locationSid);

        return stageItem;
    }

    private IDropTableScheme[] GetMonsterDropTables(MonsterCombatantPrefab[] monsterCombatantPrefabs)
    {
        var dropTables = new List<IDropTableScheme>();

        foreach (var monsterCombatantPrefab in monsterCombatantPrefabs)
        {
            switch (monsterCombatantPrefab.ClassSid)
            {
                case "digitalwolf":
                    dropTables.Add(new DropTableScheme("digital-claws",
                        new IDropTableRecordSubScheme[]
                            { new DropTableRecordSubScheme(null, new Range<int>(1, 1), "digital-claws", 1) }, 1));
                    break;

                case "chaser":
                    dropTables.Add(new DropTableScheme("bandages",
                        new IDropTableRecordSubScheme[]
                            { new DropTableRecordSubScheme(null, new Range<int>(1, 1), "bandages", 1) }, 1));
                    break;
            }
        }

        return dropTables.ToArray();
    }

    /// <inheritdoc />
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }
}