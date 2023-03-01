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
