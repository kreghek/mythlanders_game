using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core;

public interface IStoryState
{
    IReadOnlyCollection<CharacterRelation> CharacterRelations { get; }
    IReadOnlyCollection<string> Keys { get; }

    void AddCharacterRelations(UnitName name);
    void AddKey(string storySid, string key);
}

public enum CharacterKnowledgeLevel
{
    Hidden,
    Unknown,
    FullName
}

public sealed class CharacterRelation
{
    public CharacterRelation(UnitName name)
    {
        Name = name;
    }

    public CharacterKnowledgeLevel Level { get; set; }
    public UnitName Name { get; }
}