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

    private HeroCampaign CreateCampaign(ILocationSid locationSid)
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

    /// <summary>
    /// Create set of different campaigns
    /// </summary>
    public IReadOnlyList<HeroCampaign> CreateSet()
    {
        var availableLocationSids = GetAvailableLocations();

        var rollCount = Math.Min(availableLocationSids.Length, 3);

        var selectedLocations = _dice.RollFromList(availableLocationSids, rollCount).ToList();

        var list = new List<HeroCampaign>();
        foreach (var locationSid in selectedLocations)
        {
            var campaign = CreateCampaign(locationSid);

            list.Add(campaign);
        }

        return list;
    }

    private static ILocationSid[] GetAvailableLocations()
    {
        return new[]
                {
            LocationSids.Thicket,
            //LocationSids.Monastery,
            //LocationSids.ShipGraveyard,
            //LocationSids.Desert,

            //LocationSids.Swamp,

            //LocationSids.Battleground
        };
    }
}