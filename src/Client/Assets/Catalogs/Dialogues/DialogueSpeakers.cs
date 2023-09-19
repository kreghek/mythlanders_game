using System.Collections.Generic;

using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

public static class DialogueSpeakers
{
    private static readonly IDictionary<UnitName, IDialogueSpeaker> _speakers =
        new Dictionary<UnitName, IDialogueSpeaker>();

    public static IDialogueSpeaker Env { get; }

    static DialogueSpeakers()
    {
        Env = new DialogueSpeaker(UnitName.Environment);
        _speakers.Add(UnitName.Environment, Env);
    }

    public static IDialogueSpeaker Get(UnitName name)
    {
        if (_speakers.TryGetValue(name, out var speaker))
        {
            return speaker;
        }

        speaker = new DialogueSpeaker(name);
        _speakers.Add(name, speaker);

        return speaker;
    }
}