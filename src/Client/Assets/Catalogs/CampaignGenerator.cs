using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.GlobalEffects;
using Client.Core;
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

    private static IReadOnlyCollection<ICampaignReward> CreateFailurePenalties()
    {
        return new[]
        {
            new AddGlobalEffectCampaignReward(new DecreaseDamageGlobeEvent())
        };
    }

    private HeroCampaignSource CreateGrindCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateGrindShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var seed = _dice.RollD100();

        var penalties = CreateFailurePenalties();

        var campaign = new HeroCampaignSource(locationSid, campaignGraph, penalties, seed);

        return campaign;
    }

    private HeroCampaignSource CreateRescueCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateRescueShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var seed = _dice.RollD100();

        var penalties = CreateFailurePenalties();

        var campaign = new HeroCampaignSource(locationSid, campaignGraph, penalties, seed);

        return campaign;
    }

    private HeroCampaignSource CreateScoutCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateScoutShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var seed = _dice.RollD100();

        var penalties = CreateFailurePenalties();

        var campaign = new HeroCampaignSource(locationSid, campaignGraph, penalties, seed);

        return campaign;
    }

    /// <summary>
    /// Create set of different campaigns
    /// </summary>
    public IReadOnlyList<HeroCampaignSource> CreateSet(Globe currentGlobe)
    {
        var availableLocationSids = currentGlobe.CurrentAvailableLocations.ToArray();

        var rollCount = Math.Min(availableLocationSids.Length, 3);

        var selectedLocations = _dice.RollFromList(availableLocationSids, rollCount).ToList();

        var list = new List<HeroCampaignSource>();

        var availableCampaignDelegates = new List<Func<ILocationSid, Globe, HeroCampaignSource>>
        {
            CreateGrindCampaign,
            CreateScoutCampaign,
            CreateRescueCampaign
        };

        foreach (var locationSid in selectedLocations)
        {
            var rolledLocationDelegate = _dice.RollFromList(availableCampaignDelegates);

            var campaign = rolledLocationDelegate(locationSid, currentGlobe);

            list.Add(campaign);
        }

        return list;
    }
}