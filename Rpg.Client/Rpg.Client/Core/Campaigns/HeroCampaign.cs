using System;
using System.Collections.Generic;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class HeroCampaign
    {
        public IReadOnlyList<CampaignStage> CampaignStages { get; set; }

        public int CurrentStageIndex { get; set; }

        internal void CompleteCurrentStage()
        {
            CurrentStageIndex++;
        }

        public bool IsCampaignComplete => CampaignStages.Count - 1 <= CurrentStageIndex;
    }
}