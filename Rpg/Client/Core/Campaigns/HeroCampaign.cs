using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

internal sealed class HeroCampaign
{
    public HeroCampaign(ILocationSid location, IGraph<ICampaignStageItem> stages)
    {
        Location = location;
        Stages = stages;
    }

    public IGraphNode<ICampaignStageItem>? CurrentStage { get; set; }

    public ILocationSid Location { get; }

    public IGraph<ICampaignStageItem> Stages { get; }
}