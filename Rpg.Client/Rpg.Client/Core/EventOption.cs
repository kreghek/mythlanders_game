namespace Rpg.Client.Core
{
    internal sealed class EventOption
    {
        public IOptionAftermath? Aftermath { get; init; }
        public bool IsEnd { get; init; }
        public EventNode Next { get; init; }
        public string TextSid { get; init; }
    }
}