namespace CombatDicesTeam.Dialogues;

public sealed class DialogueSpeech<TParagraphConditionContext, TAftermathContext>
{
    public DialogueSpeech(IDialogueSpeaker speaker, string textSid,
        DialogueParagraphConfig<TParagraphConditionContext, TAftermathContext> config)
    {
        TextSid = textSid;
        Speaker = speaker;

        Aftermaths = config.Aftermaths;

        Conditions = config.Conditions;
    }

    public IReadOnlyCollection<IDialogueOptionAftermath<TAftermathContext>> Aftermaths { get; }

    public IReadOnlyCollection<IDialogueParagraphCondition<TParagraphConditionContext>> Conditions { get; }
    public IDialogueSpeaker Speaker { get; }
    public string TextSid { get; }
}