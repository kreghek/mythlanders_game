using CombatDicesTeam.Graphs;

namespace Client.Core.Campaigns;

/// <summary>
/// Immutable campaign source to get choise to player.
/// </summary>
internal sealed class HeroCampaignLocation
{
    public HeroCampaignLocation(ILocationSid sid, IGraph<ICampaignStageItem> stages)
    {
        Sid = sid;
        Stages = stages;
    }

    public ILocationSid Sid { get; }

    public IGraph<ICampaignStageItem> Stages { get; }
}
