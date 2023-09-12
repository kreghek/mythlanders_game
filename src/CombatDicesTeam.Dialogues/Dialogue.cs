namespace Client.Core.Dialogues;

public sealed class Dialogue
{
    public Dialogue(DialogueNode root)
    {
        Root = root;
    }

    public DialogueNode Root { get; }
}