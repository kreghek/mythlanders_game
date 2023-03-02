using System.Collections.Generic;

using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class CrisisEventCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return new NotImplemenetedStageItem("Crisis");
    }
}