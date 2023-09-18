namespace CombatDicesTeam.Dialogues;

public sealed class DialogueParagraphConfig<TParagraphConditionContext, TAftermathContext>
{
    public DialogueParagraphConfig()
    {
        Conditions = ArraySegment<IDialogueParagraphCondition<TParagraphConditionContext>>.Empty;
        Aftermaths = ArraySegment<IDialogueOptionAftermath<TAftermathContext>>.Empty;
    }

    public IReadOnlyCollection<IDialogueParagraphCondition<TParagraphConditionContext>> Conditions { get; init; }
    public IReadOnlyCollection<IDialogueOptionAftermath<TAftermathContext>> Aftermaths { get; init; }
}