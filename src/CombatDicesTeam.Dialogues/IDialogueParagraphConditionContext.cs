namespace Client.Core.Dialogues;

public interface IDialogueParagraphConditionContext
{
    public IReadOnlyCollection<string> CurrentHeroes { get; }
}