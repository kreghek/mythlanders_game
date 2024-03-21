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
internal sealed class PreHistoryDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        // Canonical story
        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] = "pre-history"
        };

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = new IDialogueEventRequirement[]
            {
                new DisabledToSelectRequirement()
            }
        };

        return new DialogueEvent(DialogueConstants.SideQuests.SynthAsParent.Sid, questStateMachine, requirements,
            dialogues);
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        return ArraySegment<IStoryPoint>.Empty;
    }
}