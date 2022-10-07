﻿using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class HeroCampaign
    {
        public IReadOnlyList<CampaignStage> CampaignStages { get; set; }

        public int CurrentStageIndex { get; set; }

        internal void CompleteCurrentStage()
        {
            CampaignStages[CurrentStageIndex].IsCompleted = true;

            CurrentStageIndex++;
        }

        public bool IsCampaignComplete => CampaignStages.All(x=>x.IsCompleted);
    }
}