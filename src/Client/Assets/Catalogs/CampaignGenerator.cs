using System.Collections.Generic;
using System.Linq;

using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Graphs.Generation.TemplateBased;

using Core.Dices;

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
        var availableLocations = new[]
        {
            LocationSids.Thicket,
            LocationSids.Monastery,
            LocationSids.ShipGraveyard,
            LocationSids.Desert,

            LocationSids.Swamp,

            LocationSids.Battleground
        };

        var selectedLocations = _dice.RollFromList(availableLocations, 3).ToList();

        var list = new List<HeroCampaign>();
        foreach (var location in selectedLocations)
        {
            var campaign = CreateCampaign(location);

            list.Add(campaign);
        }

        return list;
    }
}