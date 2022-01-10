using System;
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
        private readonly IReadOnlyCollection<ResourceItem> _inventory;
        private readonly IList<UnitScheme> _knownMonsters;
        private readonly Group _party;
        private readonly PoolGroup _pool;

        public Player()
        {
            _party = new Group();
            _pool = new PoolGroup();
            _knownMonsters = new List<UnitScheme>();

            var inventory = CreateInventory();

            _inventory = inventory;

            _abilities = new HashSet<PlayerAbility>();
        }

        public IReadOnlyCollection<PlayerAbility> Abilities => _abilities;

        public void AddPlayerAbility(PlayerAbility ability)
        {
            _abilities.Add(ability);
        }

        public bool HasAbility(PlayerAbility ability)
        {
            return _abilities.Contains(ability);
        }

        public IReadOnlyCollection<ResourceItem> Inventory => _inventory;

        public IList<UnitScheme> KnownMonsters => _knownMonsters;

        public Group Party => _party;

        public PoolGroup Pool => _pool;

        public IEnumerable<Unit> GetAll()
        {
            return Party.Slots.Where(x => x.Unit is not null).Select(x => x.Unit).Concat(Pool.Units);
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

        public void ClearAbilities()
        {
            _abilities.Clear();
        }
    }
}