﻿using Client.Assets.StageItems;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class TextEventStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly LocationSid _locationSid;
    private readonly CampaignStageTemplateServices _services;

    public TextEventStageTemplateFactory(LocationSid locationSid, CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _services = services;
    }

    public ICampaignStageItem Create()
    {
        return new TextEventStageItem("synth_as_parent_stage_1", _locationSid, _services.EventCatalog);
    }
}