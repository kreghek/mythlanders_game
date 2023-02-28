using System.Collections.Generic;

using Client.Core;
using Client.Core.Dialogues;

using Rpg.Client.Core;

namespace Client.Assets.Dialogues;

internal interface IDialogueEventFactory
{
    DialogueEvent CreateEvent(IDialogueEventFactoryServices services);
    IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services);
}