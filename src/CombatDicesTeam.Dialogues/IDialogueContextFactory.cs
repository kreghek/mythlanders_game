namespace Client.Core.Dialogues;

public interface IDialogueContextFactory<TParagraphConditionContext, TAftermathContext>
{
    TParagraphConditionContext CreateParagraphConditionContext();
    TAftermathContext CreateAftermathContext();
}