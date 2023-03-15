using System.Collections.Generic;
using System.Linq;

using Client.Core.Heroes;

namespace Rpg.Client.Core
{
    internal sealed class Group
    {
        public Group()
        {
            Slots = Enumerable
                .Range(0, 6)
                .Select(x => new GroupSlot { Index = x, IsTankLine = CheckIsTankLine(x) })
                .ToArray();
        }

        public IReadOnlyList<GroupSlot> Slots { get; }

        public IEnumerable<GroupSlot> GetFreeSlots()
        {
            return Slots
                .Where(x => x.Unit is null)
                .ToArray();
        }

        public IEnumerable<Hero> GetUnits()
        {
            return Slots
                .Where(x => x.Unit is not null)
                .Select(x => x.Unit!)
                .ToArray();
        }

        private static bool CheckIsTankLine(int slotIndex)
        {
            return slotIndex is >= 0 and < 3;
        }
    }
}