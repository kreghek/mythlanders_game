using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.CampaignRewards;

using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

/// <summary>
/// Mutable state of a campaign exploring.
/// </summary>
internal sealed class HeroCampaign
{
    public IReadOnlyCollection<HeroState> Heroes { get; }

    public HeroCampaign(IReadOnlyCollection<HeroState> heroes, HeroCampaignLocation location, IReadOnlyCollection<ICampaignReward> failurePenalties, int visualizationSeed)
    {
        Heroes = heroes;
        Location = location;

        ActualRewards = location.Stages.GetAllNodes().Select(x => x.Payload)
            .OfType<IRewardCampaignStageItem>().First().GetEstimateRewards(location);
        ActualFailurePenalties = failurePenalties;

        VisualizationSeed = visualizationSeed;
        Path = new List<IGraphNode<ICampaignStageItem>>();
    }

    public HeroCampaignLocation Location { get; }

    public IGraphNode<ICampaignStageItem>? CurrentStage { get; set; }


    public IList<IGraphNode<ICampaignStageItem>> Path { get; }

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

    public int VisualizationSeed { get; }
}