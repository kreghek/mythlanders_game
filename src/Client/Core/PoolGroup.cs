using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes;

namespace Rpg.Client.Core
{
    internal sealed class PoolGroup
    {
        private readonly IList<Hero> _units;

        public PoolGroup()
        {
            _units = new List<Hero>();
        }

        public IEnumerable<Hero> Units => _units.ToArray();

        public void AddNewUnit(Hero unit)
        {
            _units.Add(unit);
        }

        public void MoveFromGroup(Hero unit, Group targetGroup)
        {
            var slot = targetGroup.Slots.Single(x => x.Unit == unit);
            slot.Unit = null;
            _units.Add(unit);
        }

        public void MoveToGroup(Hero unit, int targetSlotIndex, Group targetGroup)
        {
            targetGroup.Slots[targetSlotIndex].Unit = unit;
            _units.Remove(unit);
        }
    }
}