using Client.Core;

namespace Client.Assets.Dialogues;

internal sealed record DialogueEventFactoryServices(IEventCatalog EventCatalog) : IDialogueEventFactoryServices;