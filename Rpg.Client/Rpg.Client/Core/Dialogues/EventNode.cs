using System.Collections.Generic;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class EventNode
    {
        public EventPosition CombatPosition { get; set; }
        public IEnumerable<EventOption> Options { get; set; }
        public EventTextBlock TextBlock { get; init; }

        private static readonly EventNode _endNode = new EventNode();
        public static EventNode EndNode => _endNode;
    }
}