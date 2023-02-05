using System.Collections.Generic;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class CampaignStage
    {
        public bool IsCompleted { get; set; }
        public IReadOnlyList<ICampaignStageItem> Items { get; set; }
    }
}