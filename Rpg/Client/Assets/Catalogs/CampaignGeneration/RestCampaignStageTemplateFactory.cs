using System.Collections.Generic;

using Client.Assets.StageItems;

using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class RestCampaignStageTemplateFactory : ICampaignStageTemplateFactory
{
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return true;
    }

    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        return new RestStageItem();
    }
}