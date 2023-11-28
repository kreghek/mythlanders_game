using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core;
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

    private HeroCampaign CreateGrindCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateGrindShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var seed = _dice.RollD100();

        var campaign = new HeroCampaign(locationSid, campaignGraph, seed);

        return campaign;
    }
    
    private HeroCampaign CreateScoutCampaign(ILocationSid locationSid, Globe globe)
    {
        var shortTemplateGraph = _wayTemplatesCatalog.CreateScoutShortTemplate(locationSid);

        var graphGenerator =
            new TemplateBasedGraphGenerator<ICampaignStageItem>(
                new TemplateConfig<ICampaignStageItem>(shortTemplateGraph));

        var campaignGraph = graphGenerator.Create();

        var seed = _dice.RollD100();

        var campaign = new HeroCampaign(locationSid, campaignGraph, seed);

        return campaign;
    }

    private static ILocationSid[] GetAvailableLocations()
    {
        return new[]
        {
            LocationSids.Thicket
            //LocationSids.Monastery,
            //LocationSids.ShipGraveyard,
            //LocationSids.Desert,

            //LocationSids.Swamp,

            //LocationSids.Battleground
        };
    }

    /// <summary>
    /// Create set of different campaigns
    /// </summary>
    public IReadOnlyList<HeroCampaign> CreateSet(Globe currentGlobe)
    {
        var availableLocationSids = currentGlobe.CurrentAvailableLocations.ToArray();

        var rollCount = Math.Min(availableLocationSids.Length, 3);

        var selectedLocations = _dice.RollFromList(availableLocationSids, rollCount).ToList();

        var list = new List<HeroCampaign>();

        var availableCampaignDelegates = new Func<ILocationSid, Globe, HeroCampaign>[]
        {
            CreateGrindCampaign,
            CreateScoutCampaign
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