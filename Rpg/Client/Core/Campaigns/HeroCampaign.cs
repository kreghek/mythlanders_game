using System.Collections.Generic;

using Rpg.Client.Core.Campaigns;

namespace Client.Core.Campaigns;

internal sealed class HeroCampaign
{
    public HeroCampaign(ILocationSid location, IReadOnlyList<CampaignStage> campaignStages)
    {
        Location = location;
        CampaignStages = campaignStages;
    }

    public IReadOnlyList<CampaignStage> CampaignStages { get; }

    public int CurrentStageIndex { get; private set; }

    public ILocationSid Location { get; }

    internal void CompleteCurrentStage()
    {
        CampaignStages[CurrentStageIndex].IsCompleted = true;

        CurrentStageIndex++;
    }
}