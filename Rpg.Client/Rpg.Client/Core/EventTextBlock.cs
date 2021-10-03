using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class EventTextBlock
    {
        public IReadOnlyList<EventTextFragment> Fragments { get; set; }
    }
}