﻿using Rpg.Client.Core.Campaigns;
using Client.Assets.StageItems;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class TrainingCampaignStageTemplate : ICampaignStageTemplate
{
    private readonly CampaignStageTemplateServices _services;

    public TrainingCampaignStageTemplate(CampaignStageTemplateServices services)
    {
        _services = services;
    }

    public ICampaignStageItem Create()
    {
        return new TrainingStageItem(_services.GlobeProvider.Globe.Player, _services.Dice);
    }
}
