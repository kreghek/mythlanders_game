namespace CombatDicesTeam.Dialogues;

public sealed class DialogueParagraphConfig<TParagraphConditionContext, TAftermathContext>
{
    public IReadOnlyCollection<IDialogueOptionAftermath<TAftermathContext>> Aftermaths { get; init; } =
        ArraySegment<IDialogueOptionAftermath<TAftermathContext>>.Empty;

    public IReadOnlyCollection<IDialogueParagraphCondition<TParagraphConditionContext>> Conditions { get; init; } =
        ArraySegment<IDialogueParagraphCondition<TParagraphConditionContext>>.Empty;
}