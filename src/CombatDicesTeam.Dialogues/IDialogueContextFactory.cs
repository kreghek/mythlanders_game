namespace CombatDicesTeam.Dialogues;

public interface IDialogueContextFactory<TParagraphConditionContext, TAftermathContext>
{
    TParagraphConditionContext CreateParagraphConditionContext();
    TAftermathContext CreateAftermathContext();
}