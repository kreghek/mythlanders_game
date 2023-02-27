using System.Collections.Generic;

using Stateless;

namespace Client.Core.Dialogues;

internal sealed class DialogueEvent
{
    private readonly IDictionary<DialogueEventState, string> _dialogueDict;
    private readonly IDictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>> _requirements;
    private readonly StateMachine<DialogueEventState, DialogueEventTrigger> _stateMachine;

    public DialogueEvent(string sid, StateMachine<DialogueEventState, DialogueEventTrigger> stateMachine,
        IDictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>> requirements,
        IDictionary<DialogueEventState, string> dialogueDict)
    {
        Sid = sid;
        _stateMachine = stateMachine;
        _requirements = requirements;
        _dialogueDict = dialogueDict;
    }

    public bool Completed => _stateMachine.State.Sid == "complete";

    public string Sid { get; }

    public string GetDialogSid()
    {
        return _dialogueDict[_stateMachine.State];
    }

    public IReadOnlyCollection<IDialogueEventRequirement> GetRequirements()
    {
        return _requirements[_stateMachine.State];
    }

    public void Trigger(DialogueEventTrigger trigger)
    {
        _stateMachine.Fire(trigger);
    }

    public bool IsStarted => _stateMachine.State.Sid == "stage_1";
}