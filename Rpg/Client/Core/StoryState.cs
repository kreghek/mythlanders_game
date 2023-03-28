using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;

namespace Client.Core;

internal sealed class StoryState : IStoryState
{
    private readonly Group _heroParty;
    private readonly IList<CharacterRelation> _relations = new List<CharacterRelation>();
    private readonly IList<string> _storyKeys = new List<string>();

    public StoryState(Group heroParty)
    {
        _heroParty = heroParty;
    }

    private IReadOnlyCollection<CharacterRelation> GetPlayerUnitsAsFullKnown(Group heroParty)
    {
        return heroParty.GetUnits().Select(x => new CharacterRelation(x.UnitScheme.Name)
            { Level = CharacterKnowledgeLevel.FullName }).ToArray();
    }

    public IReadOnlyCollection<string> Keys => _storyKeys.ToArray();

    public IReadOnlyCollection<CharacterRelation> CharacterRelations =>
        _relations.Concat(GetPlayerUnitsAsFullKnown(_heroParty)).ToArray();

    public void AddCharacterRelations(UnitName name)
    {
        throw new NotImplementedException();
    }

    public void AddKey(string storySid, string key)
    {
        throw new NotImplementedException();
    }
}