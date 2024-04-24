﻿using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.CampaignGeneration;
using Client.Assets.GlobalEffects;
using Client.Core;
using Client.Core.CampaignEffects;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.GenericRanges;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.PropDrop;

namespace Client.Assets.Catalogs;

internal sealed class CampaignGenerator : ICampaignGenerator
{
    private readonly ICharacterCatalog _characterCatalog;
    private readonly IDice _dice;
    private readonly IDropResolver _dropResolver;
    private readonly GlobeProvider _globeProvider;

    private readonly string[] _heroInDev =
    {
        nameof(UnitName.Herbalist),

        nameof(UnitName.Hoplite),
        nameof(UnitName.Engineer),

        nameof(UnitName.Priest),
        nameof(UnitName.Liberator),
        nameof(UnitName.Medjay),

        nameof(UnitName.Zoologist)
    };

    private readonly IMonsterPerkManager _monsterPerkManager;

    private readonly CampaignWayTemplatesCatalog _wayTemplatesCatalog;

    public CampaignGenerator(CampaignWayTemplatesCatalog wayTemplatesCatalog,
        IDice dice,
        IDropResolver dropResolver,
        ICharacterCatalog unitSchemeCatalog,
        IMonsterPerkManager monsterPerkManager,
        GlobeProvider globeProvider)
    {
        _wayTemplatesCatalog = wayTemplatesCatalog;
        _dice = dice;
        _dropResolver = dropResolver;
        _characterCatalog = unitSchemeCatalog;
        _monsterPerkManager = monsterPerkManager;
        _globeProvider = globeProvider;
    }

    private ILocationSid[] CalculateAvailableLocations()
    {
        return GameLocations.GetGameLocations()
            .Except(_globeProvider.Globe.Player.CurrentAvailableLocations).ToArray();
    }

    private IReadOnlyCollection<string> CalculateLockedHeroes()
    {
        return _characterCatalog.AvailableHeroes
            .Except(_globeProvider.Globe.Player.Heroes.Select(
                x => x.ClassSid))
            .Except(_heroInDev)
            .ToArray();
    }

    private HeroCampaignLocation CreateCampaignLocation(ILocationSid locationSid)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateGrindShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var campaign = new HeroCampaignLocation(locationSid, campaignGraph);

        return campaign;
    }

    private static IReadOnlyCollection<IDropTableScheme> CreateCampaignRewardResources(ILocationSid locationSid)
    {
        static IReadOnlyCollection<IDropTableScheme> GetLocationResourceDrop(string sid)
        {
            return new[]
            {
                new DropTableScheme(sid, new IDropTableRecordSubScheme[]
                {
                    new DropTableRecordSubScheme(null, GenericRange<int>.CreateMono(1), sid, 1)
                }, 1)
            };
        }

        return locationSid.ToString() switch
        {
            nameof(LocationSids.Thicket) => GetLocationResourceDrop("snow"),
            nameof(LocationSids.Desert) => GetLocationResourceDrop("sand"),
            _ => ArraySegment<IDropTableScheme>.Empty
        };
    }

    private IReadOnlyCollection<ICampaignEffect> CreateFailurePenalties()
    {
        if (_globeProvider.Globe.Player.MonsterPerks.Count() > 2)
        {
            return new ICampaignEffect[]
            {
                new AddGlobalEffectCampaignEffect(new DecreaseDamageGlobeEvent()),
                new RemoveMonsterPerkCampaignEffect(RollMonsterPerkFromPool())
            };
        }

        return new ICampaignEffect[]
        {
            new AddGlobalEffectCampaignEffect(new DecreaseDamageGlobeEvent())
        };
    }

    private IReadOnlyCollection<ICampaignEffect> CreateGatherResourcesEffect(ILocationSid targetLocationSid)
    {
        var resourcesDropTables = CreateCampaignRewardResources(targetLocationSid);
        var resources = _dropResolver.Resolve(resourcesDropTables);

        return new[]
        {
            new ResourceCampaignEffect(resources)
        };
    }

    private IReadOnlyCollection<ICampaignEffect> CreateRewards(ILocationSid locationSid)
    {
        var effectFactories = new (Func<ILocationSid, IReadOnlyCollection<ICampaignEffect>>, Func<ILocationSid, bool>)[]
        {
            (CreateGatherResourcesEffect, _ => true),
            (CreateUnlockHeroEffect, _ => CalculateLockedHeroes().Any()),
            (CreateUnlockLocationEffect, _ => CalculateAvailableLocations().Any())
        };

        var rolledEffectFactory = RollEffectFactory(effectFactories, locationSid);

        var rewards = rolledEffectFactory(locationSid);

        var perksToUnlock = _monsterPerkManager.RollLocationRewardPerks().Select(x => new MonsterPerkCampaignEffect(x));

        return rewards.Union(perksToUnlock).ToArray();
    }

    private IReadOnlyCollection<ICampaignEffect> CreateUnlockHeroEffect(ILocationSid locationSid)
    {
        var heroesToJoin = CalculateLockedHeroes();

        var rolledHero = _dice.RollFromList(heroesToJoin.ToArray());

        return new[]
        {
            new UnlockHeroCampaignEffect(rolledHero)
        };
    }

    private IReadOnlyCollection<ICampaignEffect> CreateUnlockLocationEffect(ILocationSid locationSid)
    {
        var availableLocations = CalculateAvailableLocations();
        var locationToScout = _dice.RollFromList(availableLocations);

        return new[]
        {
            new LocationCampaignEffect(locationToScout)
        };
    }

    private static IReadOnlyCollection<HeroState> RollCampaignHeroes(IEnumerable<HeroState> unlockedHeroes, IDice dice)
    {
        var openList = new List<HeroState>(unlockedHeroes);

        return dice.RollFromList(openList, 3).ToArray();
    }

    private Func<ILocationSid, IReadOnlyCollection<ICampaignEffect>> RollEffectFactory(
        IReadOnlyList<(Func<ILocationSid, IReadOnlyCollection<ICampaignEffect>>, Func<ILocationSid, bool>)>
            effectFactories,
        ILocationSid locationSid)
    {
        while (true)
        {
            var rolledEffectFactory = _dice.RollFromList(effectFactories);

            if (rolledEffectFactory.Item2(locationSid))
            {
                return rolledEffectFactory.Item1;
            }
        }
    }

    private MonsterPerk RollMonsterPerkFromPool()
    {
        return _dice.RollFromList(_globeProvider.Globe.Player.MonsterPerks.ToArray());
    }

    /// <summary>
    /// Create set of different campaigns
    /// </summary>
    public IReadOnlyList<HeroCampaignLaunch> CreateSet(Globe currentGlobe)
    {
        var availableLocationSids = currentGlobe.Player.CurrentAvailableLocations.ToArray();

        const int MAX_CAMPAIGN_LAUNCH_COUNT = 3;
        var campaignLaunchCount = Math.Min(availableLocationSids.Length, MAX_CAMPAIGN_LAUNCH_COUNT);

        var selectedLocations = _dice.RollFromList(availableLocationSids, campaignLaunchCount).ToList();

        var list = new List<HeroCampaignLaunch>();

        foreach (var locationSid in selectedLocations)
        {
            var campaignSource = CreateCampaignLocation(locationSid);

            var heroes = RollCampaignHeroes(currentGlobe.Player.Heroes.ToArray(), _dice);

            var rewards = CreateRewards(locationSid);
            var penalties = CreateFailurePenalties();

            var campaignLaunch = new HeroCampaignLaunch(campaignSource, heroes, rewards, penalties);

            list.Add(campaignLaunch);
        }

        return list;
    }
}