using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.PropDrop;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class CombatCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly IDice _dice;

    private readonly ILocationSid _locationSid;

    private readonly MonsterCombatantTempate[] _monsterCombatantTemplates;

    private readonly MonsterCombatantTempateLevel _monsterLevel;
    private readonly IMonsterPerkManager _monsterPerkManager;

    public CombatCampaignStageTemplateFactory(ILocationSid locationSid, MonsterCombatantTempateLevel monsterLevel,
        CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _monsterLevel = monsterLevel;
        _dice = services.Dice;
        _monsterPerkManager = services.MonsterPerkManager;

        var factories = CatalogHelper.GetAllFactories<ICombatTemplateFactory>();

        _monsterCombatantTemplates = factories.Select(x => x.CreateSet()).SelectMany(x => x).ToArray();
    }

    public bool IsGoalStage { get; init; }

    private MonsterCombatantTempate GetApplicableTemplate()
    {
        var templates = _monsterCombatantTemplates
            .Where(x => x.Level == _monsterLevel && x.ApplicableLocations.Any(loc => loc == _locationSid)).ToArray();

        return _dice.RollFromList(templates);
    }

    private static IEnumerable<IDropTableScheme> GetMonsterDropTables(MonsterCombatantTempate monsterCombatantPrefabs)
    {
        var dropTables = new List<IDropTableScheme>();

        foreach (var monsterCombatantPrefab in monsterCombatantPrefabs.Prefabs)
        {
            switch (monsterCombatantPrefab.ClassSid)
            {
                case "digitalwolf":
                    dropTables.Add(new DropTableScheme("digital-claws",
                        new IDropTableRecordSubScheme[]
                            { new DropTableRecordSubScheme(null, GenericRange<int>.CreateMono(1), "digital-claws", 1) },
                        1));
                    break;

                case "chaser":
                    dropTables.Add(new DropTableScheme("bandages",
                        new IDropTableRecordSubScheme[]
                            { new DropTableRecordSubScheme(null, GenericRange<int>.CreateMono(1), "bandages", 1) }, 1));
                    break;
            }
        }

        return dropTables.ToArray();
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
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
            new DropTableRecordSubScheme(null, new GenericRange<int>(1, 2), "combat-xp", 1)
        }, 1));

        var combat = new CombatSource(
            monsterCombatantTemplate.Prefabs
                .Select(x =>
                    new PerkMonsterCombatantPrefab(x, _monsterPerkManager.RollMonsterPerks(x)))
                .ToArray(),
            new CombatReward(totalDropTables.ToArray())
        );

        var combatSequence = new CombatSequence
        {
            Combats = new[] { combat }
        };
        var metadata = CombatStageHelper.CreateMetadata(combat);

        var stageItem = new CombatStageItem(_locationSid, combatSequence, metadata) { IsGoalStage = IsGoalStage };

        return stageItem;
    }

    /// <inheritdoc />
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    /// <inheritdoc />
    public IGraphNode<ICampaignStageItem> Create(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return new GraphNode<ICampaignStageItem>(Create(MapContextToCurrentStageItems(context)));
    }

    /// <inheritdoc />
    public bool CanCreate(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return CanCreate(MapContextToCurrentStageItems(context));
    }
}