using System.Collections.Generic;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.CampaignRewards;

using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

/// <summary>
/// Immutable campaign source to get choise to player.
/// </summary>
internal sealed class HeroCampaignSource
{
    public HeroCampaignSource(ILocationSid location, IGraph<ICampaignStageItem> stages,
        IReadOnlyCollection<ICampaignReward> failurePenalties, int seed)
    {
        Location = location;
        Stages = stages;
        FailurePenalties = failurePenalties;
        Seed = seed;

        Rewards = Stages.GetAllNodes().Select(x => x.Payload)
            .OfType<IRewardCampaignStageItem>().First().GetEstimateRewards(this);
    }

    public IReadOnlyCollection<ICampaignReward> FailurePenalties { get; }

    public IReadOnlyCollection<ICampaignReward> Rewards { get; }

    public ILocationSid Location { get; }

    public int Seed { get; }

    public IGraph<ICampaignStageItem> Stages { get; }
}
