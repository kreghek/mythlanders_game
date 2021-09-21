using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class Event
    {
        public int Counter { get; set; }
        public bool IsUnique { get; set; }
        public IEnumerable<EventNode> Nodes { get; init; }
        public EventNode StartNode { get; init; }

        public SystemEventMarker? SystemMarker { get; set; }
    }

    internal enum SystemEventMarker
    {
        Undefined,
        MeetArcher,
        MeetHerbalist,
        MeetPriest,
        MeetMissionary
    }
}