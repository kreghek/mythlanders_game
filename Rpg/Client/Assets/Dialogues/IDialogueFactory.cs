using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Dialogues
{
    internal interface IDialogueFactory
    {
        Event Create(IEventCatalog eventCatalog);
    }
}