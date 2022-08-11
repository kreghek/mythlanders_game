using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class PoolGroup
    {
        private readonly IList<Unit> _units;

        public PoolGroup()
        {
            _units = new List<Unit>();
        }

        public IEnumerable<Unit> Units => _units.ToArray();

        public void AddNewUnit(Unit unit)
        {
            _units.Add(unit);
        }

        public void MoveFromGroup(Unit unit, Group targetGroup)
        {
            var slot = targetGroup.Slots.Single(x => x.Unit == unit);
            slot.Unit = null;
            _units.Add(unit);
        }

        public void MoveToGroup(Unit unit, int targetSlotIndex, Group targetGroup)
        {
            targetGroup.Slots[targetSlotIndex].Unit = unit;
            _units.Remove(unit);
        }
    }
}