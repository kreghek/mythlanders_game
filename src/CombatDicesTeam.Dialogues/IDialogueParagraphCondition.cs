namespace Client.Core.Dialogues;

public interface IDialogueParagraphCondition<TParagraphConditionContext>
{
    bool Check(TParagraphConditionContext context);
}