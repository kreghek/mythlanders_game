using System.Collections.Generic;

using Client.Core.Dialogues;

using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Core
{
    internal interface IEventCatalog
    {
        IEnumerable<DialogueEvent> Events { get; }

        Dialogue GetDialogue(string sid);
    }
}