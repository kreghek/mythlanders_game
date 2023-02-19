using System;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.Dialogues;

using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class SideStoryTextEventStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly LocationSid _locationSid;
    private readonly CampaignStageTemplateServices _services;

    public SideStoryTextEventStageTemplateFactory(LocationSid locationSid, CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _services = services;
    }

    /// <inheritdoc/>
    public bool CanCreate()
    {
        var availableStoies = _services.EventCatalog.Events.Where(x => MeetRequirements(x)).ToArray();

        return availableStoies.Any();
    }

    private bool MeetRequirements(Event textEvent)
    {
        return textEvent.GetRequirements().All(r => r.IsApplicableFor(_services.GlobeProvider.Globe, _locationSid));
    }

    /// <inheritdoc/>
    public ICampaignStageItem Create()
    {
        var availableStoies = _services.EventCatalog.Events.Where(x => MeetRequirements(x)).ToArray();

        var rolledStory = _services.Dice.RollFromList(availableStoies);

        if (rolledStory.Sid is null)
        {
            throw new InvalidOperationException();
        }

        return new TextEventStageItem(rolledStory.Sid, _locationSid, _services.EventCatalog);
    }
}