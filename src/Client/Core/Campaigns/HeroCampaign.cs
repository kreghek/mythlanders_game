using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.CampaignRewards;

using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

internal sealed class HeroCampaign
{
    public HeroCampaign(ILocationSid location, IGraph<ICampaignStageItem> stages, IReadOnlyCollection<ICampaignReward> failurePenalties, int seed)
    {
        Location = location;
        Stages = stages;
        FailurePenalties = failurePenalties;
        Seed = seed;

        Path = new List<IGraphNode<ICampaignStageItem>>();
    }

    public IGraphNode<ICampaignStageItem>? CurrentStage { get; set; }

    public ILocationSid Location { get; }

    public IList<IGraphNode<ICampaignStageItem>> Path { get; }
    public int Seed { get; }

    public IGraph<ICampaignStageItem> Stages { get; }

    public IReadOnlyCollection<ICampaignReward> GetCampaignRewards()
    {
        return Stages.GetAllNodes().Select(x => x.Payload)
            .OfType<IRewardCampaignStageItem>().First().GetEstimateRewards(this);
    }

    public IReadOnlyCollection<ICampaignReward> FailurePenalties { get; }
}