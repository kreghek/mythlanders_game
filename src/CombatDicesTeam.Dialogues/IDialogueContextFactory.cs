namespace CombatDicesTeam.Dialogues;

public interface IDialogueContextFactory<out TParagraphConditionContext, out TAftermathContext>
{
    TAftermathContext CreateAftermathContext();
    TParagraphConditionContext CreateParagraphConditionContext();
}