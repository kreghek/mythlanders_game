namespace Client.Core.Dialogues;

public interface IDialogueParagraphCondition
{
    bool Check(IDialogueParagraphConditionContext context);
}