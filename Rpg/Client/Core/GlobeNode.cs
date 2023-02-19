using Client.Core.Dialogues;

using Rpg.Client.Core.Campaigns;

namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public CombatSequence? AssignedCombats { get; set; }
        public Event? AssignedEvent { get; private set; }

        public BiomeType BiomeType { get; init; }
        public HeroCampaign? Campaign { get; set; }

        public EquipmentItemType? EquipmentItem { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsLast { get; internal set; }

        public LocationSid Sid { get; set; }

        public void AssignEvent(Event locationEvent)
        {
            AssignedEvent = locationEvent;
        }

        public void ClearNodeState()
        {
            AssignedEvent = null;
            AssignedCombats = null;
        }
    }
}