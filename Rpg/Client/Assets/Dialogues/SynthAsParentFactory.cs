using System.Collections.Generic;

using Client.Core.Dialogues;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Assets.Dialogues;
using Rpg.Client.Core;

using Stateless;

namespace Client.Assets.Dialogues;

internal sealed class SynthAsParentFactory : IDialogueFactory
{
    public Event Create(IEventCatalog eventCatalog)
    {
        var initialState = new EventState("start");

        var questStateMachine = new StateMachine<EventState, string>(initialState);
        var requirements = new Dictionary<EventState, IReadOnlyCollection<ITextEventRequirement>>();
        requirements[initialState] = new[] {
            new LocationEventRequirement(new[] { LocationSid.Desert })
        };

        var dialogues = new Dictionary<EventState, string>();
        dialogues[initialState] = "synth_as_parent_stage_1";

        return new Event("synth_as_parent", questStateMachine, requirements, dialogues);
    }
}