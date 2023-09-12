using System;
using System.Collections.Generic;

namespace Client.Core.Dialogues;

public sealed class DialogueSpeech
{
    public DialogueSpeech(IDialogueSpeaker speaker, string textSid, DialogueParagraphConfig config)
    {
        TextSid = textSid;
        Speaker = speaker;

        EnvironmentEffects = config.EnvironmentEffects;

        Conditions = config.Conditions;
    }

    public IReadOnlyCollection<IDialogueParagraphCondition> Conditions { get; }

    public IReadOnlyCollection<IDialogueEnvironmentEffect> EnvironmentEffects { get; }
    public IDialogueSpeaker Speaker { get; }
    public string TextSid { get; }
}