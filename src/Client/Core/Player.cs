using System;
using System.Collections.Generic;
using System.Linq;

using Client.Assets.Catalogs.Dialogues;

namespace Client.Core;

internal sealed class Player
{
    private readonly HashSet<PlayerAbility> _abilities;
    private readonly HashSet<ILocationSid> _locations;

    private readonly IList<MonsterPerk> _monsterPerks = new List<MonsterPerk>();

    public Player(string name) : this()
    {
        Name = name;
    }

    public Player()
    {
        Heroes = new PoolGroup<HeroState>();
        KnownMonsters = new List<UnitScheme>();

        Inventory = new Inventory();

        _abilities = new HashSet<PlayerAbility>();

        Name = CreateRandomName();

        StoryState = new StoryState(Heroes);
        StoryState.AddCharacterRelations(UnitName.Radio);
        StoryState.CharacterRelations.Single(x => x.Character.Equals(DialogueSpeakers.Get(UnitName.Radio))).Level = CharacterKnowledgeLevel.FullName;

        _locations = new HashSet<ILocationSid>();
    }

    public IReadOnlyCollection<PlayerAbility> Abilities => _abilities;
    public IChallenge? Challenge { get; set; }

    public IReadOnlyList<ILocationSid> CurrentAvailableLocations => _locations.ToArray();

    public PoolGroup<HeroState> Heroes { get; }

    public Inventory Inventory { get; }

    public IList<UnitScheme> KnownMonsters { get; }

    public IReadOnlyCollection<MonsterPerk> MonsterPerks => _monsterPerks.ToArray();

    public string Name { get; }
    public IStoryState StoryState { get; }

    public void AddHero(HeroState heroState)
    {
        Heroes.AddNewUnit(heroState);
    }

    public void AddLocation(ILocationSid location)
    {
        _locations.Add(location);
    }

    public void AddMonsterPerk(MonsterPerk perk)
    {
        _monsterPerks.Add(perk);
    }

    public void AddPlayerAbility(PlayerAbility ability)
    {
        _abilities.Add(ability);
    }

    public void ClearAbilities()
    {
        _abilities.Clear();
    }

    public void ClearInventory()
    {
        foreach (var resourceItem in Inventory.CalcActualItems())
        {
            Inventory.Remove(resourceItem);
        }
    }

    public bool HasAbility(PlayerAbility ability)
    {
        return _abilities.Contains(ability);
    }

    internal void RemoveMonsterPerk(MonsterPerk perk)
    {
        _monsterPerks.Remove(perk);
    }

    private static string CreateRandomName()
    {
        var first = CreditsResource.NicknameFirstParts.Split(',').Select(x => x.Trim()).ToArray();
        var last = CreditsResource.NicknameSecondParts.Split(',').Select(x => x.Trim()).ToArray();

        var rnd = new Random();

        var x = rnd.Next(0, first.Length);
        var y = rnd.Next(0, last.Length);

        return first[x] + " " + last[y];
    }
}