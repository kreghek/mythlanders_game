using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Core;

internal interface IEventCatalog
{
    IEnumerable<DialogueEvent> Events { get; }

    Dialogue<ParagraphConditionContext, CampaignAftermathContext> GetDialogue(string sid);
}