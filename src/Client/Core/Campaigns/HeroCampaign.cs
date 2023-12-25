using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.CampaignRewards;

using CombatDicesTeam.Combats;
using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

/// <summary>
/// Mutable state of a campaign exploring.
/// </summary>
internal sealed class HeroCampaign
{
    public HeroCampaign(IReadOnlyCollection<HeroState> heroes, HeroCampaignLocation location,
        IReadOnlyCollection<ICampaignReward> failurePenalties, int visualizationSeed)
    {
        Heroes = CreateCampaignHeroes(heroes);
        Location = location;

        ActualRewards = location.Stages.GetAllNodes().Select(x => x.Payload)
            .OfType<IRewardCampaignStageItem>().First().GetEstimateRewards(location);
        ActualFailurePenalties = failurePenalties;

        VisualizationSeed = visualizationSeed;
        Path = new List<IGraphNode<ICampaignStageItem>>();
    }

    private IReadOnlyCollection<HeroCampaignState> CreateCampaignHeroes(IReadOnlyCollection<HeroState> heroes)
    {
        var list = new List<HeroCampaignState>();

        var openList = new List<FieldCoords>
        {
            new FieldCoords(0,0),
            new FieldCoords(0,1),
            new FieldCoords(0,2),
            new FieldCoords(1,0),
            new FieldCoords(1,1),
            new FieldCoords(1,2),
            
        };

        foreach (var hero in heroes)
        {
            var rolledPosition = dice
            list.Add(new HeroCampaignState(hero, new FormationSlot()));
        }
        
        return list;
    }

    /// <summary>
    /// Effect which will apply if heroes fail campaign.
    /// Can be modified during campaign.
    /// </summary>
    public IReadOnlyCollection<ICampaignReward> ActualFailurePenalties { get; }

    /// <summary>
    /// Effect which will apply if heroes win campaign.
    /// Can be modified during campaign.
    /// </summary>
    //TODO Add modifiers of effects
    public IReadOnlyCollection<ICampaignReward> ActualRewards { get; }

    public IGraphNode<ICampaignStageItem>? CurrentStage { get; set; }
    public IReadOnlyCollection<HeroCampaignState> Heroes { get; }

    public HeroCampaignLocation Location { get; }


    public IList<IGraphNode<ICampaignStageItem>> Path { get; }

    public int VisualizationSeed { get; }
}