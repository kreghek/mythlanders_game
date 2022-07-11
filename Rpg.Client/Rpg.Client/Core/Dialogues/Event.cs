using System.Collections.Generic;

namespace Rpg.Client.Core.Dialogues
{
    internal sealed class Event
    {
        public string? AfterCombatStartNodeSid { get; init; }
        public string? BeforeCombatStartNodeSid { get; init; }
        public bool Completed => Counter > 0;
        public int Counter { get; set; }
        public bool IsGameStart { get; init; }
        public bool IsUnique { get; init; }
        public TextEventPriority Priority { get; init; }
        public IReadOnlyCollection<ITextEventRequirement>? Requirements { get; init; }
        public string? Sid { get; init; }
    }
}