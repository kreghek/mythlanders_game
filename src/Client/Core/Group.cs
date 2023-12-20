using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes;

namespace Client.Core;

internal sealed class Group<T>
{
    public Group()
    {
        Slots = Enumerable
            .Range(0, 6)
            .Select(x => new GroupSlot<T> { Index = x })
            .ToArray();
    }

    public IReadOnlyList<GroupSlot<T>> Slots { get; }

    public IEnumerable<GroupSlot<T>> GetFreeSlots()
    {
        return Slots
            .Where(x => x.Hero is null)
            .ToArray();
    }

    public IEnumerable<T> GetUnits()
    {
        return Slots
            .Where(x => x.Hero is not null)
            .Select(x => x.Hero!)
            .ToArray();
    }
}