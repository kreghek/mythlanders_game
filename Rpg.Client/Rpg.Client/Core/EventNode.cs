using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class EventNode
    {
        public IEnumerable<EventOption> Options { get; set; }
        public string Text { get; init; }
    }
}