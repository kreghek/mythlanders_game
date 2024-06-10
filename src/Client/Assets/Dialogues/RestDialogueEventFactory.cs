using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueEventRequirements;
using Client.Core;

using CombatDicesTeam.Dialogues;

using JetBrains.Annotations;

using Stateless;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class RestDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        const string RESOURCE_FILE_SID = "rest";
        const string DIALOGUE_SID = RESOURCE_FILE_SID;

        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = new[] { new DisabledToSelectRequirement() }
        };

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] = DIALOGUE_SID,
        };

        return new DialogueEvent(RESOURCE_FILE_SID, questStateMachine, requirements,
            dialogues);
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        return ArraySegment<IStoryPoint>.Empty;
    }
}
