using System.Collections.Generic;

using Client.Core.Dialogues;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class EventTextBlock
    {
        public EventTextBlock(IReadOnlyList<EventTextFragment> fragments)
        {
            Fragments = fragments;
        }

        public IReadOnlyList<EventTextFragment> Fragments { get; }
    }
}