using CombatDicesTeam.Dialogues;

namespace Client.Core;

public sealed class CharacterRelation
{
    public CharacterRelation(IDialogueSpeaker character)
    {
        Character = character;
    }

    public CharacterKnowledgeLevel Level { get; set; }
    public IDialogueSpeaker Character { get; }
}