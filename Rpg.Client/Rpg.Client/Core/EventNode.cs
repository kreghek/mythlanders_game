using System.Collections.Generic;

namespace Rpg.Client.Core
{
    internal sealed class EventNode
    {
        public EventPosition CombatPosition { get; set; }
        public IEnumerable<EventOption> Options { get; set; }
        public EventTextBlock TextBlock { get; init; }
    }

    internal sealed class EventTextBlock
    {
        public IReadOnlyList<EventTextFragment> Fragments { get; set; }
    }

    internal sealed class EventTextFragment
    {
        public EventSpeaker Speaker { get; init; }
        public string TextSid { get; init; }
    }

    internal enum EventSpeaker
    {
        Undefined = 0,
        Environment,
        Berimir,
        Hawk
    }

    internal enum EventPosition
    {
        Undefined,
        BeforeCombat,
        AfterCombat
    }
}