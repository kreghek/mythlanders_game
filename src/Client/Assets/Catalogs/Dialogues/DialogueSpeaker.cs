using Client.Core;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Catalogs.Dialogues;

public sealed record DialogueSpeaker(UnitName Name) : IDialogueSpeaker
{
    public override string ToString()
    {
        return Name.ToString();
    }
}