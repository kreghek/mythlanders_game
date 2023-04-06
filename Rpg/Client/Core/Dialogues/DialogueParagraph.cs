using System;
using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core.Dialogues;

internal sealed class DialogueParagraph
{
    public DialogueParagraph(UnitName speaker, string textSid, DialogueParagraphConfig config)
    {
        TextSid = textSid;
        Speaker = speaker;

        EnvironmentEffects = config.EnvironmentEffects;

        Conditions = config.Conditions;
    }

    public IReadOnlyCollection<IDialogueEnvironmentEffect> EnvironmentEffects { get; }
    public UnitName Speaker { get; }
    public string TextSid { get; }

    public IReadOnlyCollection<IDialogueParagraphCondition> Conditions { get; }
}

internal sealed class DialogueParagraphConfig
{
    public IReadOnlyCollection<IDialogueParagraphCondition> Conditions { get; init; }
    public IReadOnlyCollection<IDialogueEnvironmentEffect> EnvironmentEffects { get; init; }

    public DialogueParagraphConfig()
    {
        Conditions = ArraySegment<IDialogueParagraphCondition>.Empty;
        EnvironmentEffects = ArraySegment<IDialogueEnvironmentEffect>.Empty;
    }
}

internal interface IDialogueParagraphCondition
{
    bool Check(IDialogueParagraphConditionContext context);
}

internal interface IDialogueParagraphConditionContext
{
    public IReadOnlyCollection<string> CurrentHeroes { get; }
}