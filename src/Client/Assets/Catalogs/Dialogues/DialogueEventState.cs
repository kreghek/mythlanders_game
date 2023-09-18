namespace Client.Assets.Catalogs.Dialogues;

public sealed record DialogueEventState(string Sid)
{
    public bool NoDialogue { get; init; }
}