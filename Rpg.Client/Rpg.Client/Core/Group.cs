using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    internal sealed class Group
    {
        public Group()
        {
            Slots = Enumerable
                .Range(0, 6)
                .Select(x => new GroupSlot{ Index = x })
                .ToArray();
        }

        public IReadOnlyList<GroupSlot> Slots { get; }

        public IEnumerable<Unit> GetUnits() => Slots
            .Where(x => x.Unit is not null)
            .Select(x => x.Unit!)
            .ToArray();
        
        public IEnumerable<GroupSlot> GetFreeSlots() => Slots
            .Where(x => x.Unit is null)
            .ToArray();
    }

    internal sealed class PoolGroup
    {
        public PoolGroup()
        {
            Units = Array.Empty<Unit>();
        }

        public IEnumerable<Unit> Units { get; set; }
    }

    internal sealed class GroupSlot
    {
        public Unit? Unit { get; set; }
        public int Index { get; init; }
    }
}