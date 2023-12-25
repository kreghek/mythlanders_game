using System.Collections.Generic;
using System.Linq;

namespace Client.Core;

internal sealed class PoolGroup<T> where T : class
{
    private readonly IList<T> _units;

    public PoolGroup()
    {
        _units = new List<T>();
    }

    public IEnumerable<T> Units => _units.ToArray();

    public void AddNewUnit(T unit)
    {
        _units.Add(unit);
    }

    public void MoveFromGroup(T unit, Group<T> targetGroup)
    {
        var slot = targetGroup.Slots.Single(x => x.Hero == unit);
        slot.Hero = null;
        _units.Add(unit);
    }

    public void MoveToGroup(T unit, int targetSlotIndex, Group<T> targetGroup)
    {
        targetGroup.Slots[targetSlotIndex].Hero = unit;
        _units.Remove(unit);
    }
}