using System.Collections.Generic;

using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

internal sealed class HeroCampaign
{
    public HeroCampaign(ILocationSid location, IGraph<ICampaignStageItem> stages, int seed)
    {
        Location = location;
        Stages = stages;
        Seed = seed;

        Path = new List<IGraphNode<ICampaignStageItem>>();
    }

    public IGraphNode<ICampaignStageItem>? CurrentStage { get; set; }

    public ILocationSid Location { get; }

    public IList<IGraphNode<ICampaignStageItem>> Path { get; }
    public int Seed { get; }

    public IGraph<ICampaignStageItem> Stages { get; }
}