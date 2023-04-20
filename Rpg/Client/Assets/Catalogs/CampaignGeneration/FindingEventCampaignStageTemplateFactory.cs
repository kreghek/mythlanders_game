using System.Collections.Generic;

using Client.Assets.StageItems;
using Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class FindingEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return new FindingStageItem();
    }
}