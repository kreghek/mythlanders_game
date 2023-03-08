using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal sealed class DialogueParagraph
{
    public DialogueParagraph(UnitName speaker, string textSid)
    {
        TextSid = textSid;
        Speaker = speaker;

        EnvironmentEffects = Array.Empty<IDialogueEnvironmentEffect>();
    }

    public IReadOnlyCollection<IDialogueEnvironmentEffect> EnvironmentEffects { get; init; }
    public UnitName Speaker { get; }
    public string TextSid { get; }
}