﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Combats.CombatantEffectLifetimes;
using CombatDicesTeam.Combats.CombatantStatuses;
using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.Combats.CombatantStatuses;
using Core.PropDrop;

using GameAssets.Combats;
using GameAssets.Combats.CombatantStatuses;

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
        var locationTemplates = _monsterCombatantTemplates
            .Where(x => x.ApplicableLocations.Any(loc => loc == _locationSid)).ToArray();

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

    private static IReadOnlyCollection<TFactory> LoadCombatTemplateFactories<TFactory>()
    {
        var assembly = typeof(TFactory).Assembly;
        var factoryTypes = assembly.GetTypes()
            .Where(x => typeof(TFactory).IsAssignableFrom(x) && x != typeof(TFactory) && !x.IsAbstract);
        var factories = factoryTypes.Select(Activator.CreateInstance);
        return factories.OfType<TFactory>().ToArray();
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    private static readonly ICombatantStatusFactory[] _availablePerkBuffs = {
        new DelegateCombatStatusFactory(()=>new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(new CombatantStatusSid("HP"), new OwnerBoundCombatantEffectLifetime(), CombatantStatTypes.HitPoints, 1))),
        new DelegateCombatStatusFactory(()=>new AutoRestoreModifyStatCombatantStatus(new ModifyStatCombatantStatus(new CombatantStatusSid("SP"), new OwnerBoundCombatantEffectLifetime(), CombatantStatTypes.ShieldPoints, 1))),
    };

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
                .Select(x => new PerkMonsterCombatantPrefab(x, RollPerks(_availablePerkBuffs, _dice))).ToArray(),
            new CombatReward(totalDropTables.ToArray())
        );

        var combatSequence = new CombatSequence
        {
            Combats = new[] { combat }
        };

        var stageItem = new CombatStageItem(_locationSid, combatSequence);

        return stageItem;
    }

    private static IReadOnlyCollection<ICombatantStatusFactory> RollPerks(IReadOnlyCollection<ICombatantStatusFactory> availablePerkBuffs,
        IDice dice)
    {
        var count = dice.Roll(0, availablePerkBuffs.Count);

        if (count < 0)
        {
            throw new InvalidOperationException("Rolled perk count can't be below zero.");
        }
        
        if (count == 0)
        {
            return ArraySegment<ICombatantStatusFactory>.Empty;
        }

        return dice.RollFromList(availablePerkBuffs.ToArray(), count).ToArray();
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