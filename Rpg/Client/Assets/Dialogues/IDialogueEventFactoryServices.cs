using Rpg.Client.Core;

namespace Client.Assets.Dialogues;

internal interface IDialogueEventFactoryServices
{
    IEventCatalog EventCatalog { get; }
}