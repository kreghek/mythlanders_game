using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Player
    {
        public Player()
        {
            Pool = new Group();
        }

        public Group Group { get; set; }

        public Group Pool { get; set; }

        public void MoveToParty(Unit unit)
        {
            if (Group.Units.Count() >= 3)
            {
                throw new InvalidOperationException("Party limit was reached.");
            }

            var partyList = new List<Unit>(Group.Units);
            var poolList = new List<Unit>(Pool.Units);

            poolList.Remove(unit);
            partyList.Add(unit);

            Group.Units = partyList;
            Pool.Units = poolList;
        }

        public void MoveToPool(Unit unit)
        {
            var partyList = new List<Unit>(Group.Units);
            var poolList = new List<Unit>(Pool.Units);

            partyList.Remove(unit);
            poolList.Add(unit);

            Group.Units = partyList;
            Pool.Units = poolList;
        }

        public IEnumerable<Unit> GetAll => Group.Units.Concat(Pool.Units);
    }
}