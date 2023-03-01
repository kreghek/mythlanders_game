﻿using System;
using System.Collections.Generic;
using System.Linq;

using Rpg.Client.Core;

namespace Client.Core;

internal sealed class StoryState : IStoryState
{
    private readonly IList<CharacterRelation> _relations = new List<CharacterRelation>();
    private readonly IList<string> _storyKeys = new List<string>();
    private readonly Group _heroParty;

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

        var inventory = CreateInventory();

        Inventory = inventory;

        _abilities = new HashSet<PlayerAbility>();

        Name = CreateRandomName();

        StoryState = new StoryState(Party);
    }

    public IReadOnlyCollection<PlayerAbility> Abilities => _abilities;

    public IReadOnlyCollection<ResourceItem> Inventory { get; }

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
        foreach (var resourceItem in Inventory)
        {
            resourceItem.Amount = 0;
        }
    }

    public IEnumerable<Unit> GetAll()
    {
        var unitsInSlots = Party.Slots.Where(x => x.Unit is not null).Select(x => x.Unit!);
        return unitsInSlots.Concat(Pool.Units);
    }

    public bool HasAbility(PlayerAbility ability)
    {
        return _abilities.Contains(ability);
    }

    public void MoveToParty(Unit unit, int slotIndex)
    {
        Pool.MoveToGroup(unit, slotIndex, Party);
    }

    public void MoveToPool(Unit unit)
    {
        Pool.MoveFromGroup(unit, Party);
    }

    private static IReadOnlyCollection<ResourceItem> CreateInventory()
    {
        var inventoryAvailableItems = Enum.GetValues<EquipmentItemType>();

        return inventoryAvailableItems.Select(enumItem => new ResourceItem(enumItem)).ToList();
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