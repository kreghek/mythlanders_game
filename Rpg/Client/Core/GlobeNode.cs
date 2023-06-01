﻿using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Dialogues;

namespace Rpg.Client.Core
{
    internal sealed class GlobeNode
    {
        public CombatSequence? AssignedCombats { get; set; }
        public DialogueEvent? AssignedEvent { get; private set; }

        public BiomeCulture BiomeType { get; init; }
        public HeroCampaign? Campaign { get; set; }

        public EquipmentItemType? EquipmentItem { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsLast { get; internal set; }

        public ILocationSid Sid { get; set; }

        public void AssignEvent(DialogueEvent locationEvent)
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