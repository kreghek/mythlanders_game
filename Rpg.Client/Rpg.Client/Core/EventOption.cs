namespace Rpg.Client.Core
{
    internal sealed class EventOption
    {
        public EventOption(string textSid, EventNode nextNode)
        {
            TextSid = textSid;
            Next = nextNode;
        }

        public IOptionAftermath? Aftermath { get; init; }
        public EventNode Next { get; }
        public string TextSid { get; }
    }
}