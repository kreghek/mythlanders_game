using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
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
        }

        public IReadOnlyCollection<PlayerAbility> Abilities => _abilities;

        public IReadOnlyCollection<ResourceItem> Inventory { get; }

        public IList<UnitScheme> KnownMonsters { get; }

        public string Name { get; }

        public Group Party { get; }

        public PoolGroup Pool { get; }

        public void AddPlayerAbility(PlayerAbility ability)
        {
            _abilities.Add(ability);
        }

        public void ClearAbilities()
        {
            _abilities.Clear();
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
            var poolList = new List<Unit>(Pool.Units);

            poolList.Remove(unit);
            Party.Slots[slotIndex].Unit = unit;

            Pool.Units = poolList;
        }

        public void MoveToPool(Unit unit)
        {
            var poolList = new List<Unit>(Pool.Units);

            var slot = Party.Slots.Single(x => x.Unit == unit);
            slot.Unit = null;
            poolList.Add(unit);

            Pool.Units = poolList;
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
}