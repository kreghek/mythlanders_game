using System.Collections.Generic;

using Client.Core.Dialogues;

using Rpg.Client.Core.Dialogues;

namespace Client.Core;

internal interface IEventCatalog
{
    IEnumerable<DialogueEvent> Events { get; }

    Dialogue GetDialogue(string sid);
}