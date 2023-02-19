using System.Collections.Generic;

using Client.Core.Dialogues;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Core;

using Stateless;

namespace Client.Assets.Dialogues;

internal sealed class SynthAsParentDualogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent Create(IEventCatalog eventCatalog)
    {
        var initialState = new DialogueEventState("start");

        var questStateMachine = new StateMachine<DialogueEventState, DialogueEventTrigger>(initialState);
        questStateMachine.Configure(initialState)
            .Permit(new DialogueEventTrigger("stage_1_ignore"), new DialogueEventState("complete"))
            .Permit(new DialogueEventTrigger("stage_1_fast"), new DialogueEventState("stage_2_fast"))
            .Permit(new DialogueEventTrigger("stage_1_help"), new DialogueEventState("stage_2"));

        questStateMachine.Configure(new DialogueEventState("stage_2_fast"))
            .Permit(new DialogueEventTrigger("stage_2_complete"), new DialogueEventState("complete"));

        questStateMachine.Configure(new DialogueEventState("stage_2"))
            .Permit(new DialogueEventTrigger("stage_2_complete"), new DialogueEventState("complete"));

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [initialState] = new[] 
            {
                new LocationEventRequirement(new[] { LocationSid.Desert })
            }
        };

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [initialState] = "synth_as_parent_stage_1"
        };

        return new DialogueEvent("synth_as_parent", questStateMachine, requirements, dialogues);
    }
}