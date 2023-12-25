using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.StageItems;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dices;
using CombatDicesTeam.Graphs;
using CombatDicesTeam.Graphs.Generation.TemplateBased;

namespace Client.Assets.Catalogs.CampaignGeneration;

internal sealed class SideStoryDialogueEventStageTemplateFactory : ICampaignStageTemplateFactory
{
    private readonly ILocationSid _locationSid;
    private readonly CampaignStageTemplateServices _services;

    public SideStoryDialogueEventStageTemplateFactory(ILocationSid locationSid, CampaignStageTemplateServices services)
    {
        _locationSid = locationSid;
        _services = services;
    }

    private DialogueEvent[] GetAvailableStories()
    {
        var requirementContext =
            new DialogueEventRequirementContext(_services.GlobeProvider.Globe, _locationSid, _services.EventCatalog);

        var availableStories =
            _services.EventCatalog.Events.Where(x => MeetRequirements(x, requirementContext)).ToArray();
        return availableStories;
    }

    private static ICampaignStageItem[] MapContextToCurrentStageItems(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return context.CurrentWay.Select(x => x.Payload).ToArray();
    }

    private static bool MeetRequirements(DialogueEvent textEvent, DialogueEventRequirementContext requirementContext)
    {
        var dialogueEventRequirements = textEvent.GetRequirements();
        return dialogueEventRequirements.All(r => r.IsApplicableFor(requirementContext));
    }

    /// <inheritdoc />
    public bool CanCreate(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        if (currentStageItems.OfType<DialogueEventStageItem>().Any())
        {
            return false;
        }

        var availableStories = GetAvailableStories();

        return availableStories.Any();
    }

    /// <inheritdoc />
    public ICampaignStageItem Create(IReadOnlyList<ICampaignStageItem> currentStageItems)
    {
        var availableStoies = GetAvailableStories();

        var rolledStory = _services.Dice.RollFromList(availableStoies);

        if (rolledStory.Sid is null)
        {
            throw new InvalidOperationException();
        }

        return new DialogueEventStageItem(rolledStory.Sid, _locationSid, _services.EventCatalog);
    }

    /// <inheritdoc />
    public IGraphNode<ICampaignStageItem> Create(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return new GraphNode<ICampaignStageItem>(Create(MapContextToCurrentStageItems(context)));
    }

    /// <inheritdoc />
    public bool CanCreate(IGraphTemplateContext<ICampaignStageItem> context)
    {
        return CanCreate(MapContextToCurrentStageItems(context));
    }
}