using System;
using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes;

using Core.Combats;

using Rpg.Client.Core;

namespace Client.Core;

internal sealed class Player
{
    private readonly HashSet<PlayerAbility> _abilities;

    public Player(string name) : this()
    {
        Name = name;

        CampaignHeroStates = new[]
        {
            new HeroCampaignState("swordsman", new StatValue(5)),
            new HeroCampaignState("partisan", new StatValue(4)),
            new HeroCampaignState("amazon", new StatValue(3))
        };
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

    /// <summary>
    /// Data of the hero in the current campaign.
    /// </summary>
    //TODO Move in the campaign
    public IReadOnlyCollection<HeroCampaignState> CampaignHeroStates { get; }

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

    public IEnumerable<Hero> GetAll()
    {
        var unitsInSlots = Party.Slots.Where(x => x.Unit is not null).Select(x => x.Unit!);
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