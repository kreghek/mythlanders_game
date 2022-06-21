using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class EventNode
    {
        public EventPosition CombatPosition { get; set; }
        public static EventNode EndNode { get; } = new EventNode();

        public IEnumerable<EventOption> Options { get; set; }
        public EventTextBlock TextBlock { get; init; }
    }
}