using System.Collections.Generic;

namespace Client.Core;

public interface IStoryState
{
    IReadOnlyCollection<CharacterRelation> CharacterRelations { get; }
    IReadOnlyCollection<string> Keys { get; }

    void AddCharacterRelations(UnitName name);
    void AddKey(string key);
}