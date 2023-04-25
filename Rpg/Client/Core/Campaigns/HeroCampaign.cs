using Rpg.Client.Core.Campaigns;

namespace Client.Core.Campaigns;

internal sealed class HeroCampaign
{
    public HeroCampaign(ILocationSid location, ICampaignGraph<ICampaignStageItem> stages)
    {
        Location = location;
        Stages = stages;
    }

    public ICampaignGraph<ICampaignStageItem> Stages { get; }

    public ICampaignGraphNode<ICampaignStageItem>? CurrentStage { get; private set; }

    public ILocationSid Location { get; }

    internal void CompleteCurrentStage()
    {
        CurrentStage.Value.IsCompleted = true;
    }
}