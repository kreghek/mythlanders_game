namespace Client.Core.Dialogues;

public sealed record DialogueEventState(string Sid)
{
    public bool NoDialogue { get; init; }
}