﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal enum PlayerAbility
    {
        SkipTutorials,
        ReadMapTutorial,
        ReadEventTutorial,
        ReadCombatTutorial,
        AvailableTanks
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
            return Party.Slots.Where(x => x.Unit is not null).Select(x => x.Unit).Concat(Pool.Units);
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

        private static List<ResourceItem> CreateInventory()
        {
            var inventory = new List<ResourceItem>();
            var inventoryAvailableItems = Enum.GetValues<EquipmentItemType>();
            foreach (var enumItem in inventoryAvailableItems)
            {
                var item = new ResourceItem
                {
                    Type = enumItem
                };

                inventory.Add(item);
            }

            return inventory;
        }

        private static string CreateRandomName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}