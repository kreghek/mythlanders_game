using System.Collections.Generic;

using Stateless;

namespace Client.Core.Dialogues;

internal sealed class Event
{
    private readonly StateMachine<EventState, string> _stateMachine;
    private readonly IDictionary<EventState, IReadOnlyCollection<ITextEventRequirement>> _requirements;
    private readonly IDictionary<EventState, string> _dialogueDict;

    public Event(string sid, StateMachine<EventState,
        string> stateMachine,
        IDictionary<EventState, IReadOnlyCollection<ITextEventRequirement>> requirements,
        IDictionary<EventState, string> dialogueDict)
    {
        Sid = sid;
        _stateMachine = stateMachine;
        _requirements = requirements;
        _dialogueDict = dialogueDict;
    }

    public string GetDialogSid()
    {
        return _dialogueDict[_stateMachine.State];
    }

    public bool Completed => _stateMachine.State.Sid == "complete";
    public IReadOnlyCollection<ITextEventRequirement> GetRequirements()
    {
        return _requirements[_stateMachine.State];
    }

    public string Sid { get; }
}

internal sealed record EventState(string Sid);