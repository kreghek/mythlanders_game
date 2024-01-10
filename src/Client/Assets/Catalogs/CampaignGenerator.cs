using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.GlobalEffects;
using Client.Core;
using Client.Core.CampaignEffects;
using Client.Core.CampaignRewards;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs;

internal sealed class CampaignGenerator : ICampaignGenerator
{
    private readonly IDice _dice;
    private readonly CampaignWayTemplatesCatalog _wayTemplatesCatalog;

    public CampaignGenerator(CampaignWayTemplatesCatalog wayTemplatesCatalog, IDice dice)
    {
        _wayTemplatesCatalog = wayTemplatesCatalog;
        _dice = dice;
    }

    private static IReadOnlyCollection<ICampaignEffect> CreateFailurePenalties()
    {
        return new[]
        {
            new AddGlobalEffectCampaignReward(new DecreaseDamageGlobeEvent())
        };
    }

    private HeroCampaignLocation CreateGrindCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateGrindShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var campaign = new HeroCampaignLocation(locationSid, campaignGraph);

        return campaign;
    }

    private HeroCampaignLocation CreateRescueCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateRescueShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var campaign = new HeroCampaignLocation(locationSid, campaignGraph);

        return campaign;
    }

    private HeroCampaignLocation CreateScoutCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateScoutShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var campaign = new HeroCampaignLocation(locationSid, campaignGraph);

        return campaign;
    }

    private static IReadOnlyCollection<HeroState> RollHeroes(IReadOnlyCollection<HeroState> heroes, IDice dice)
    {
        var openList = new List<HeroState>(heroes);

        return dice.RollFromList(openList, 3).ToArray();
    }

    /// <summary>
    /// Create set of different campaigns
    /// </summary>
    public IReadOnlyList<HeroCampaignLaunch> CreateSet(Globe currentGlobe)
    {
        var availableLocationSids = currentGlobe.Player.CurrentAvailableLocations.ToArray();

        var rollCount = Math.Min(availableLocationSids.Length, 3);

        var selectedLocations = _dice.RollFromList(availableLocationSids, rollCount).ToList();

        var list = new List<HeroCampaignLaunch>();

        var availableCampaignDelegates = new List<Func<ILocationSid, Globe, HeroCampaignLocation>>
        {
            CreateGrindCampaign,
            CreateScoutCampaign,
            CreateRescueCampaign
        };

        foreach (var locationSid in selectedLocations)
        {
            var rolledLocationDelegate = _dice.RollFromList(availableCampaignDelegates);

            var campaignSource = rolledLocationDelegate(locationSid, currentGlobe);

            var heroes = RollHeroes(currentGlobe.Player.Heroes.Units.ToArray(), _dice);

            var penalties = CreateFailurePenalties();

            var campaignLaunch = new HeroCampaignLaunch(campaignSource, heroes, penalties);

            list.Add(campaignLaunch);
        }

        return list;
    }
}