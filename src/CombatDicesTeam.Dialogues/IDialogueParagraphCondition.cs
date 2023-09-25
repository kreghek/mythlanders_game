namespace CombatDicesTeam.Dialogues;

public interface IDialogueParagraphCondition<in TParagraphConditionContext>
{
    bool Check(TParagraphConditionContext context);
}