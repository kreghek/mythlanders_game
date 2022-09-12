using System.Collections.Generic;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class HeroCampaign
    {
        public IReadOnlyList<CampaignStage> CampaignStages { get; set; }
        public int CurrentStageIndex { get; set; }

    }
}