namespace CombatDicesTeam.Dialogues;

public interface IDialogueParagraphCondition<TParagraphConditionContext>
{
    bool Check(TParagraphConditionContext context);
}