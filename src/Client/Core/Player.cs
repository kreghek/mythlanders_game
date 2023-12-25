using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.Core;

internal sealed class Player
{
    private readonly HashSet<PlayerAbility> _abilities;

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
    }

    public IReadOnlyCollection<PlayerAbility> Abilities => _abilities;
    public IChallenge? Challenge { get; set; }

    public Inventory Inventory { get; }

    public IList<UnitScheme> KnownMonsters { get; }

    public string Name { get; }

    public PoolGroup<HeroState> Heroes { get; }
    public IStoryState StoryState { get; }

    public void AddHero(HeroState heroState)
    {
        Heroes.AddNewUnit(heroState);
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