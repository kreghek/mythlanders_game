namespace CombatDicesTeam.Dialogues;

public sealed class DialogueParagraph<TParagraphConditionContext, TAftermathContext>
{
    public DialogueParagraph(IReadOnlyList<DialogueSpeech<TParagraphConditionContext, TAftermathContext>> paragraphs)
    {
        Paragraphs = paragraphs;
    }

    public IReadOnlyList<DialogueSpeech<TParagraphConditionContext, TAftermathContext>> Paragraphs { get; }
}