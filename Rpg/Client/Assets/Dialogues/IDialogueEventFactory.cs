using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client.Assets.Dialogues;

internal interface IDialogueEventFactory
{
    DialogueEvent Create(IEventCatalog eventCatalog);
}