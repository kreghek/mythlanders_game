using System.Collections.Generic;

using Client.Core.Campaigns;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class CampaignStage
    {
        public bool IsCompleted { get; set; }
        public IReadOnlyList<ICampaignStageItem> Items { get; set; }
    }
}