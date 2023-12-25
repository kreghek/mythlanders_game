using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

namespace Client.Assets.Dialogues;

internal interface IDialogueEventFactory
{
    DialogueEvent CreateEvent(IDialogueEventFactoryServices services);
    IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services);
}