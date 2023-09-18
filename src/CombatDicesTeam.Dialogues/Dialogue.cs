namespace CombatDicesTeam.Dialogues;

public sealed class Dialogue<TParagraphConditionContext, TAftermathContext>
{
    public Dialogue(DialogueNode<TParagraphConditionContext, TAftermathContext> root)
    {
        Root = root;
    }

    public DialogueNode<TParagraphConditionContext, TAftermathContext> Root { get; }
}