using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class EventNode
    {
        public EventPosition CombatPosition { get; set; }
        public IEnumerable<EventOption> Options { get; set; }
        public EventTextBlock TextBlock { get; init; }
    }
}