﻿using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class HeroCampaign
    {
        public HeroCampaign(GlobeNodeSid location, IReadOnlyList<CampaignStage> campaignStages)
        {
            Location = location;
            CampaignStages = campaignStages;
        }

        public IReadOnlyList<CampaignStage> CampaignStages { get; }

        public int CurrentStageIndex { get; private set; }

        public bool IsCampaignComplete => CampaignStages.All(x => x.IsCompleted);
        public GlobeNodeSid Location { get; }

        internal void CompleteCurrentStage()
        {
            CampaignStages[CurrentStageIndex].IsCompleted = true;

            CurrentStageIndex++;
        }
    }
}