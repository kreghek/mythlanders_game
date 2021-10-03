namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public GlobeNode(string name)
        {
            Name = name;
        }

        public Event? AssignedEvent { get; set; }

        public bool IsAvailable { get; set; }

        public CombatSequence? CombatSequence { get; set; }

        public EquipmentItemType? EquipmentItem { get; set; }

        public int Index { get; internal set; }

        public string Name { get; }

        public GlobeNodeSid Sid { get; set; }
    }
}