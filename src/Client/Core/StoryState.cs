using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;
using Client.Core.Heroes;

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

    private static CharacterRelation CreateFullyKnownRelations(Hero hero)
    {
        return new CharacterRelation(new DialogueSpeaker(hero.UnitScheme.Name))
        {
            Level = CharacterKnowledgeLevel.FullName
        };
    }

    private static IReadOnlyCollection<CharacterRelation> GetPlayerUnitsAsFullKnown(Group heroParty)
    {
        var heroes = heroParty.GetUnits();
        return heroes.Select(CreateFullyKnownRelations).ToArray();
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