using System.Collections.Generic;

using Client.Core.CampaignRewards;

using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

/// <summary>
/// Mutable state of a campaign exploring.
/// </summary>
internal sealed class HeroCampaign
{
    public IReadOnlyCollection<HeroState> Heroes { get; }

    public HeroCampaign(IReadOnlyCollection<HeroState> heroes, HeroCampaignSource source)
    {
        Heroes = heroes;
        Source = source;

        ActualRewards = source.Rewards;
        ActualFailurePenalties = Source.FailurePenalties;

        Path = new List<IGraphNode<ICampaignStageItem>>();
    }

    public HeroCampaignSource Source { get; }

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

    public int Seed => Source.Seed;
}