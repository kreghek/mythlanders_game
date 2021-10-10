namespace Rpg.Client.Core.EventSerialization
{
    internal sealed record EventStorageModel
    {
        public string Location { get; set; }
        public string Name { get; set; }

        public EventNodeStorageModel BeforeCombatNode { get; set; }
        public EventNodeStorageModel AfterCombatNode { get; set; }
    }

    internal sealed record EventNodeStorageModel
    {
        public string Speaker { get; set; }
        public string Text { get; set; }
        public EventNodeStorageModel NextNode { get; set; }
    }
}
