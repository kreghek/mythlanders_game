using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes;

namespace Client.Core;

internal sealed class Group
{
    public Group()
    {
        Slots = Enumerable
            .Range(0, 6)
            .Select(x => new GroupSlot { Index = x })
            .ToArray();
    }

    public IReadOnlyList<GroupSlot> Slots { get; }

    public IEnumerable<GroupSlot> GetFreeSlots()
    {
        return Slots
            .Where(x => x.Hero is null)
            .ToArray();
    }

    public IEnumerable<Hero> GetUnits()
    {
        return Slots
            .Where(x => x.Hero is not null)
            .Select(x => x.Hero!)
            .ToArray();
    }
}