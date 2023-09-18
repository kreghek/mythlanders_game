namespace CombatDicesTeam.Dialogues;

public interface IDialogueParagraphConditionContext
{
    public IReadOnlyCollection<string> CurrentHeroes { get; }
}