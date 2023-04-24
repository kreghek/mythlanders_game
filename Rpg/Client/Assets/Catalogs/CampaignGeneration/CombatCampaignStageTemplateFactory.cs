using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using Core.Combats;
using Core.Dices;
using Core.PropDrop;

using Rpg.Client.Core;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class CombatCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly IDice _dice;

    private readonly ILocationSid _locationSid;

    private readonly MonsterCombatantTempate[] _monsterCombatantTemplates;

    private readonly MonsterCombatantTempateLevel _monsterLevel;

    public CombatCampaignStageTemplateFactory(ILocationSid locationSid, MonsterCombatantTempateLevel monsterLevel,
        CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _monsterLevel = monsterLevel;
        _dice = services.Dice;

        var factories = LoadCombatTemplateFactories<ICombatTemplateFactory>();

        _monsterCombatantTemplates = factories.Select(x => x.CreateSet()).SelectMany(x => x).ToArray();
    }

    private MonsterCombatantTempate GetApplicableTemplate()
    {
        var templates = _monsterCombatantTemplates
            .Where(x => x.Level == _monsterLevel && x.ApplicableLocations.Any(a => a == _locationSid)).ToArray();

        return _dice.RollFromList(templates);
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

    private static IReadOnlyCollection<TFactory> LoadCombatTemplateFactories<TFactory>()
    {
        var assembly = typeof(TFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(TFactory).IsAssignableFrom(x) && x != typeof(TFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<TFactory>().ToArray();
    }

    /// <inheritdoc />
    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var monsterCombatantTemplate = GetApplicableTemplate();

        var monsterResources = GetMonsterDropTables(monsterCombatantTemplate);

        var totalDropTables = new List<IDropTableScheme>();
        totalDropTables.AddRange(monsterResources);
        totalDropTables.Add(new DropTableScheme("combat-xp", new IDropTableRecordSubScheme[]
        {
            new DropTableRecordSubScheme(null, new Range<int>(1, 2), "combat-xp", 1)
        }, 1));

        var combat = new CombatSource(
            monsterCombatantTemplate.Prefabs,
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