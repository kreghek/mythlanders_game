namespace Client.Core.Dialogues;

public sealed class DialogueParagraphConfig
{
    public DialogueParagraphConfig()
    {
        Conditions = ArraySegment<IDialogueParagraphCondition>.Empty;
        EnvironmentEffects = ArraySegment<IDialogueEnvironmentEffect>.Empty;
    }

    public IReadOnlyCollection<IDialogueParagraphCondition> Conditions { get; init; }
    public IReadOnlyCollection<IDialogueEnvironmentEffect> EnvironmentEffects { get; init; }
}