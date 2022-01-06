namespace Rpg.Client.Core.EventSerialization
{
    internal sealed record EventStorageModel
    {
        public EventNodeStorageModel AfterCombatNode { get; set; }
        public string? Aftermath { get; set; }

        public EventNodeStorageModel BeforeCombatNode { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Sid { get; set; }
    }
}