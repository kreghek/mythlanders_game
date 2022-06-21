using System.Collections.Generic;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class EventTextBlock
    {
        public IReadOnlyList<EventTextFragment> Fragments { get; set; }
    }
}