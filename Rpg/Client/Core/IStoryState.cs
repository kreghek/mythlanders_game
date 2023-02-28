using System.Collections.Generic;

using Rpg.Client.Core;

namespace Client.Core;

public interface IStoryState
{
    IReadOnlyCollection<string> Keys { get; }
    void AddKey(string storySid, string key);

    void AddCharacterRelations(UnitName name);
    IReadOnlyCollection<CharacterRelation> CharacterRelations { get; }
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