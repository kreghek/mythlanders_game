using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

using Stateless;

namespace Client.Assets.Dialogues;

internal abstract class SmallEventDialogueEventFactoryBase : IDialogueEventFactory
{
    protected abstract string DialogueFileSid { get; }
    protected abstract string EventSid { get; }

    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = Array.Empty<IDialogueEventRequirement>()
        };

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] = $"{DialogueFileSid}_crisis"
        };

        return new DialogueEvent($"{EventSid}Event", questStateMachine, requirements,
            dialogues);
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        return Array.Empty<IStoryPoint>();
    }
}