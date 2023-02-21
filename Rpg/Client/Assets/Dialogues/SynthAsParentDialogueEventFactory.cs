using System.Collections.Generic;

using Client.Assets.StoryPointAftermaths;
using Client.Core.Dialogues;

using JetBrains.Annotations;

using Rpg.Client.Assets.DialogueEventRequirements;
using Rpg.Client.Assets.StoryPointJobs;
using Rpg.Client.Core;

using Stateless;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class SynthAsParentDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        var initialState = new DialogueEventState("start");

        var questStateMachine = new StateMachine<DialogueEventState, DialogueEventTrigger>(initialState);
        questStateMachine.Configure(initialState)
            .Permit(new DialogueEventTrigger("stage_1_ignore"), new DialogueEventState("complete"))
            .Permit(new DialogueEventTrigger("stage_1_fast"), new DialogueEventState("stage_1_fast"))
            .Permit(new DialogueEventTrigger("stage_1_help"), new DialogueEventState("stage_1_help"));

        questStateMachine.Configure(new DialogueEventState("stage_1_fast"))
            .Permit(new DialogueEventTrigger("stage_1_complete"), new DialogueEventState("complete"));

        questStateMachine.Configure(new DialogueEventState("stage_1_help"))
            .Permit(new DialogueEventTrigger("stage_1_complete"), new DialogueEventState("stage_2"));

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

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        var spList = new List<IStoryPoint>();
        
        var synthStage1Story = new StoryPoint("synth_as_parent_stage_1")
        {
            TitleSid = "synth_as_parent",
            CurrentJobs = new[]
            {
                new Job("Победа над противниками", "{0}: {1}/{2}", "{0} - завершено")
                {
                    Scheme = new JobScheme
                    {
                        Scope = JobScopeCatalog.Global,
                        Type = JobTypeCatalog.Defeats,
                        Value = 12
                    }
                }
            },
            Aftermaths = new IStoryPointAftermath[]
            {
                new TriggerQuestStoryPointAftermath("synth_as_parent", new DialogueEventTrigger("stage_1_complete"), services.EventCatalog)
            }
        };

        spList.Add(synthStage1Story);

        return spList;
    }
}