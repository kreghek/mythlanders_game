using System.Collections.Generic;
using System.Linq;

using Core.Dices;

using Rpg.Client.Assets.Perks;
using Rpg.Client.Assets.StageItems;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

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

    public ICampaignStageItem Create()
    {
        var combat = new CombatSource
        {
            Level = 1,
            EnemyGroup = new Group()
        };

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

        for (var slotIndex = 0; slotIndex < monsterInfos.Count; slotIndex++)
        {
            var scheme = _services.UnitSchemeCatalog.AllMonsters.Single(x => x.Name == monsterInfos[slotIndex].name);
            combat.EnemyGroup.Slots[slotIndex].Unit = new Unit(scheme, monsterInfos[slotIndex].level);
        }

        return stageItem;
    }

    private IReadOnlyList<(UnitName name, int level)> GetStartMonsterInfoList(LocationSid location)
    {
        var availableAllRegularMonsters = _services.UnitSchemeCatalog.AllMonsters.Where(x => !HasPerk<BossMonster>(x, 1));

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

        var units = new List<Unit>();
        foreach (var unitScheme in rolledUnits)
        {
            var unitLevel = 2;
            var unit = new Unit(unitScheme, unitLevel);
            units.Add(unit);
        }

        return rolledUnits.Select(x => (x.Name, 2)).ToArray();
    }

    private static bool HasPerk<TPerk>(UnitScheme unitScheme, int combatLevel)
    {
        var unit = new Unit(unitScheme, combatLevel);
        return unit.Perks.OfType<TPerk>().Any();
    }

    public bool CanCreate()
    {
        return true;
    }
}