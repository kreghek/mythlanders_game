using CombatDicesTeam.Dialogues;

namespace Client.Core;

public sealed class CharacterRelation
{
    public CharacterRelation(IDialogueSpeaker character)
    {
        Character = character;
    }

    public IDialogueSpeaker Character { get; }

    public CharacterKnowledgeLevel Level { get; set; }
}