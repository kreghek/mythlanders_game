using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Rpg.Client.Assets.Dialogues
{
    internal interface IDialogueFactory
    {
        Event Create(IEventCatalog eventCatalog);
    }
}
