namespace CombatDicesTeam.Dialogues;

public sealed class DialogueNode<TParagraphConditionContext, TAftermathContext>
{
    static DialogueNode()
    {
        var dialogueParagraph =
            new DialogueParagraph<TParagraphConditionContext, TAftermathContext>(
                Array.Empty<DialogueSpeech<TParagraphConditionContext, TAftermathContext>>());

        EndNode = new DialogueNode<TParagraphConditionContext, TAftermathContext>(
            dialogueParagraph,
            Array.Empty<DialogueOption<TParagraphConditionContext, TAftermathContext>>());
    }

    public DialogueNode(DialogueParagraph<TParagraphConditionContext, TAftermathContext> textBlock,
        IReadOnlyCollection<DialogueOption<TParagraphConditionContext, TAftermathContext>> options)
    {
        Options = options;
        TextBlock = textBlock;
    }

    public static DialogueNode<TParagraphConditionContext, TAftermathContext> EndNode { get; }

    public IReadOnlyCollection<DialogueOption<TParagraphConditionContext, TAftermathContext>> Options { get; }

    public DialogueParagraph<TParagraphConditionContext, TAftermathContext> TextBlock { get; }
}