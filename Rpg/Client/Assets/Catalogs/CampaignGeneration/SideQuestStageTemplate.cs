using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Client.Assets.StageItems;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class SideQuestStageTemplate : ICampaignStageTemplate
{
    private readonly GlobeNodeSid _locationSid;
    private readonly CampaignStageTemplateServices _services;

    public SideQuestStageTemplate(GlobeNodeSid locationSid, CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _services = services;
    }

    public ICampaignStageItem Create()
    {
        return new TextEventStageItem("synth_as_parent_stage_1", _locationSid, _services.EventCatalog);
    }
}
