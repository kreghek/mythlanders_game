using System.Collections.Generic;

using CombatDicesTeam.Dialogues;

using Stateless;

namespace Client.Assets.Catalogs.Dialogues;

/// <summary>
/// Dialog event.
/// The Event is composition of the multiple Dialogues each starts according the inner state of the Event.
/// </summary>
public sealed class DialogueEvent
{
    private readonly IDictionary<DialogueEventState, string> _dialogueDict;
    private readonly IDictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>> _requirements;
    private readonly StateMachine<DialogueEventState, DialogueEventTrigger> _stateMachine;

    public DialogueEvent(string sid,
        StateMachine<DialogueEventState, DialogueEventTrigger> stateMachine,
        IDictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>> requirements,
        IDictionary<DialogueEventState, string> dialogueDict)
    {
        Sid = sid;
        _stateMachine = stateMachine;
        _requirements = requirements;
        _dialogueDict = dialogueDict;
    }

    public bool Completed => _stateMachine.State.Sid == "complete";

    public bool IsStarted => _stateMachine.State.Sid != "stage_1";

    public string Sid { get; }

    public string GetDialogSid()
    {
        return _dialogueDict[_stateMachine.State];
    }

    public IReadOnlyCollection<IDialogueEventRequirement> GetRequirements()
    {
        if (_stateMachine.State.NoDialogue)
        {
            return new[] { new IsInProgressEventRequirement() };
        }

        return _requirements[_stateMachine.State];
    }

    public void Trigger(DialogueEventTrigger trigger)
    {
        _stateMachine.Fire(trigger);
    }

    private sealed class IsInProgressEventRequirement : IDialogueEventRequirement
    {
        public bool IsApplicableFor(IDialogueEventRequirementContext context)
        {
            // Quests in progress is not available to be a base of campaign stage.
            return false;
        }
    }

    public sealed class IsNotCrisesRequirement : IDialogueEventRequirement
    {
        public bool IsApplicableFor(IDialogueEventRequirementContext context)
        {
            return false;
        }
    }
}