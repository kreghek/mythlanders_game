using System;
using System.Linq;

using Client.Assets.StageItems;
using Client.Core.Dialogues;

using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class SideStoryDialogueEventStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly LocationSid _locationSid;
    private readonly CampaignStageTemplateServices _services;

    public SideStoryDialogueEventStageTemplateFactory(LocationSid locationSid, CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _services = services;
    }

    private static bool MeetRequirements(DialogueEvent textEvent, DialogueEventRequirementContext requirementContext)
    {
        return textEvent.GetRequirements().All(r => r.IsApplicableFor(requirementContext));
    }

    /// <inheritdoc />
    public bool CanCreate()
    {
        var availableStoies = GetAvailableStories();

        return availableStoies.Any();
    }

    private DialogueEvent[] GetAvailableStories()
    {
        var requirementContext = new DialogueEventRequirementContext(_services.GlobeProvider.Globe, _locationSid, _services.EventCatalog);

        var availableStories = _services.EventCatalog.Events.Where(x => MeetRequirements(x, requirementContext)).ToArray();
        return availableStories;
    }

    /// <inheritdoc />
    public ICampaignStageItem Create()
    {
        var availableStoies = GetAvailableStories();

        var rolledStory = _services.Dice.RollFromList(availableStoies);

        if (rolledStory.Sid is null)
        {
            throw new InvalidOperationException();
        }

        return new DialogueEventStageItem(rolledStory.Sid, _locationSid, _services.EventCatalog);
    }
}