using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

public sealed record DialogueSpeaker(UnitName Name) : IDialogueSpeaker, IEqualityComparer<DialogueSpeaker>
{
    public bool Equals(DialogueSpeaker? x, DialogueSpeaker? y)
    {
        return x?.Name == y?.Name;
    }

    public int GetHashCode([DisallowNull] DialogueSpeaker obj)
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}