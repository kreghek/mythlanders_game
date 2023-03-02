namespace Rpg.Client.Core.Dialogues
{
    internal sealed class DialogueOption
    {
        public DialogueOption(string textSid, DialogueNode nextNode)
        {
            TextSid = textSid;
            Next = nextNode;
        }

        public IDialogueOptionAftermath? Aftermath { get; init; }
        public DialogueNode Next { get; }
        public string TextSid { get; }
    }
}