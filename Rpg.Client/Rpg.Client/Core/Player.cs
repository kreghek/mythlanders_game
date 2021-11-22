using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Player
    {
        public Player()
        {
            Party = new Group();
            Pool = new PoolGroup();
        }

        public Group Party { get; }

        public PoolGroup Pool { get; }

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
    }
}