using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Core;

internal sealed class StoryState : IStoryState
{
    private readonly PoolGroup<HeroState> _heroParty;
    private readonly IList<CharacterRelation> _relations = new List<CharacterRelation>();
    private readonly IList<string> _storyKeys = new List<string>();

    public StoryState(PoolGroup<HeroState> heroParty)
    {
        _heroParty = heroParty;
    }

    private static CharacterRelation CreateFullyKnownRelations(UnitName heroName)
    {
        return new CharacterRelation(new DialogueSpeaker(heroName))
        {
            Level = CharacterKnowledgeLevel.FullName
        };
    }

    private static IReadOnlyCollection<CharacterRelation> GetPlayerUnitsAsFullKnown(PoolGroup<HeroState> heroParty)
    {
        var heroes = heroParty.Units.Select(x => Enum.Parse<UnitName>(x.ClassSid, true));
        return heroes.Select(CreateFullyKnownRelations).ToArray();
    }

    public IReadOnlyCollection<string> Keys => _storyKeys.ToArray();

    public IReadOnlyCollection<CharacterRelation> CharacterRelations =>
        _relations.Concat(GetPlayerUnitsAsFullKnown(_heroParty)).ToArray();

    public void AddCharacterRelations(UnitName name)
    {
        _relations.Add(new CharacterRelation(DialogueSpeakers.Get(name))
        {
            Level = CharacterKnowledgeLevel.Hidden
        });
    }

    public void AddKey(string key)
    {
        _storyKeys.Add(key);
    }
}