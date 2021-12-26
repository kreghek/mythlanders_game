namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public Event? AssignedEvent { get; set; }

        public CombatSequence? CombatSequence { get; set; }

        public EquipmentItemType? EquipmentItem { get; set; }

        public bool IsAvailable { get; set; }

        public GlobeNodeSid Sid { get; set; }
        public bool IsLast { get; internal set; }
    }
}