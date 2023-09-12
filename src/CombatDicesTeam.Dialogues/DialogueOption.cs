namespace Client.Core.Dialogues;

public sealed class DialogueOption<TAftermathContext>
{
    public DialogueOption(string textSid, DialogueNode nextNode)
    {
        TextSid = textSid;
        Next = nextNode;
    }

    public IDialogueOptionAftermath<TAftermathContext>? Aftermath { get; init; }
    public DialogueNode Next { get; }
    public string TextSid { get; }
}