using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

public sealed record DialogueSpeaker(UnitName Name) : IDialogueSpeaker, IEqualityComparer<DialogueSpeaker>, IEqualityComparer<IDialogueSpeaker>
{
    public bool Equals(DialogueSpeaker? x, DialogueSpeaker? y)
    {
        return x?.Name == y?.Name;
    }

    public bool Equals(IDialogueSpeaker? x, IDialogueSpeaker? y)
    {
        if (x is DialogueSpeaker s1 && y is DialogueSpeaker s2)
        { 
            return Equals(s1, s2);
        }

        return false;
    }

    public int GetHashCode([DisallowNull] DialogueSpeaker obj)
    {
        return Name.GetHashCode();
    }

    public int GetHashCode([DisallowNull] IDialogueSpeaker obj)
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name.ToString();
    }
}