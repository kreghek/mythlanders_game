namespace Client.Core.Dialogues;

internal sealed class Dialogue
{
    public Dialogue(DialogueNode root)
    {
        Root = root;
    }

    public DialogueNode Root { get; }
}