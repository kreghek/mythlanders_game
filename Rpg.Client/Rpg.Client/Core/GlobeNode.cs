using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public Event? AssignedEvent { get; set; }

        public BiomeType BiomeType { get; init; }

        public CombatSequence? AssignedCombats { get; set; }

        public EquipmentItemType? EquipmentItem { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsLast { get; internal set; }

        public GlobeNodeSid Sid { get; set; }
    }
}