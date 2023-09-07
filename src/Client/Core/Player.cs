using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes;

using CombatDicesTeam.Combats;

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
        Party = new Group();
        Pool = new PoolGroup();
        KnownMonsters = new List<UnitScheme>();

        Inventory = new Inventory();

        _abilities = new HashSet<PlayerAbility>();

        Name = CreateRandomName();

        StoryState = new StoryState(Party);

        Heroes = new[]
        {
            new HeroState("swordsman", new StatValue(5), new FieldCoords(0, 1)),
            new HeroState("partisan", new StatValue(3), new FieldCoords(1, 0)),
            new HeroState("robber", new StatValue(3), new FieldCoords(1, 2))
        };
    }

    public IReadOnlyCollection<PlayerAbility> Abilities => _abilities;

    public IReadOnlyCollection<HeroState> Heroes { get; }

    public Inventory Inventory { get; }

    public IList<UnitScheme> KnownMonsters { get; }

    public string Name { get; }

    public Group Party { get; }

    public PoolGroup Pool { get; }
    public IStoryState StoryState { get; }

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

    public IEnumerable<Hero> GetAll()
    {
        var unitsInSlots = Party.Slots.Where(x => x.Hero is not null).Select(x => x.Hero!);
        return unitsInSlots.Concat(Pool.Units);
    }

    public bool HasAbility(PlayerAbility ability)
    {
        return _abilities.Contains(ability);
    }

    public void MoveToParty(Hero unit, int slotIndex)
    {
        Pool.MoveToGroup(unit, slotIndex, Party);
    }

    public void MoveToPool(Hero unit)
    {
        Pool.MoveFromGroup(unit, Party);
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