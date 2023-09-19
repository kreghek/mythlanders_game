namespace CombatDicesTeam.Dialogues;

public interface IDialogueContextFactory<TParagraphConditionContext, TAftermathContext>
{
    TAftermathContext CreateAftermathContext();
    TParagraphConditionContext CreateParagraphConditionContext();
}