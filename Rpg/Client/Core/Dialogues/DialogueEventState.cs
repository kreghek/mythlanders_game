namespace Client.Core.Dialogues;

internal sealed record DialogueEventState(string Sid)
{
    public bool NoDialogue { get; init; }
};