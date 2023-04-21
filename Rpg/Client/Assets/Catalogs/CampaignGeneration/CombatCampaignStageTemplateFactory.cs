using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using Core.Combats;
using Core.PropDrop;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class CombatCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly LocationSid _locationSid;

    private readonly MonsterCombatantTempate[] _monsterCombatantTemplates =
    {
        new MonsterCombatantTempate(
            new MonsterCombatantTempateLevel(0),
            new[]{ LocationSid.Thicket },
        new MonsterCombatantPrefab[]
        {
            new("aspid", 0, new FieldCoords(0, 1)),
            //new ("chaser", 0, new FieldCoords(0, 1))
            //new ("volkolakwarrior", 0, new FieldCoords(1, 2)),
            //new ("chaser", 1, new FieldCoords(1, 2)),
            new("digitalwolf", 0, new FieldCoords(0, 2))
        }),

        new MonsterCombatantTempate(
            new MonsterCombatantTempateLevel(1),
            new[]{ LocationSid.Thicket },
            new MonsterCombatantPrefab[]
        {
            new("aspid", 0, new FieldCoords(0, 1)),
            //new ("chaser", 0, new FieldCoords(0, 1))
            //new ("volkolakwarrior", 0, new FieldCoords(1, 2)),
            //new ("chaser", 1, new FieldCoords(1, 2)),
            new("digitalwolf", 0, new FieldCoords(1, 1))
        }),

        new MonsterCombatantTempate(
            new MonsterCombatantTempateLevel(2),
            new[]{ LocationSid.Thicket },
            new MonsterCombatantPrefab[]
        {
            new("aspid", 0, new FieldCoords(0, 1)),
            new("digitalwolf", 0, new FieldCoords(0, 2)),
            new("digitalwolf", 0, new FieldCoords(1, 1))
        })
    };

    private readonly MonsterCombatantTempateLevel _monsterLevel;

    public CombatCampaignStageTemplateFactory(LocationSid locationSid, MonsterCombatantTempateLevel monsterLevel)
    {
        _locationSid = locationSid;
        _monsterLevel = monsterLevel;
    }

    private static IDropTableScheme[] GetMonsterDropTables(MonsterCombatantTempate monsterCombatantPrefabs)
    {
        var dropTables = new List<IDropTableScheme>();

        foreach (var monsterCombatantPrefab in monsterCombatantPrefabs.Prefabs)
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
    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var monsterCombatantPrefabs = _monsterCombatantTemplates.Single(x => x.Level == _monsterLevel && x.ApplicableLocations.Any(a => a == _locationSid));

        var monsterResources = GetMonsterDropTables(monsterCombatantPrefabs);

        var totalDropTables = new List<IDropTableScheme>();
        totalDropTables.AddRange(monsterResources);
        totalDropTables.Add(new DropTableScheme("combat-xp", new IDropTableRecordSubScheme[]
        {
            new DropTableRecordSubScheme(null, new Range<int>(1, 2), "combat-xp", 1)
        }, 1));

        var combat = new CombatSource(
            monsterCombatantPrefabs.Prefabs,
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

        return stageItem;
    }

    /// <inheritdoc />
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }
}