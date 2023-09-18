using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

public static class DialogueSpeakers
{
    static DialogueSpeakers()
    {
        Env = new DialogueSpeaker(UnitName.Environment);
        _speakers.Add(UnitName.Environment, Env);
    }

    private static readonly IDictionary<UnitName, IDialogueSpeaker> _speakers = new Dictionary<UnitName, IDialogueSpeaker>();
    
    public static IDialogueSpeaker Get(UnitName name)
    {
        if (!_speakers.TryGetValue(name, out var speaker))
        {
            speaker = new DialogueSpeaker(name);
        }

        return speaker;
    }

    public static IDialogueSpeaker Env;
}