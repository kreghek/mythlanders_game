using System.Collections.Generic;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class CampaignStage
    {
        public IReadOnlyList<ICampaignStageItem> Items { get; set; }
        public string Title { get; set; }
    }
}