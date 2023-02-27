using System.Collections.Generic;

using Client.Assets.DialogueEventRequirements;
using Client.Assets.StoryPointAftermaths;
using Client.Core;
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
        var initialState = new DialogueEventState("stage_1");

        var questStateMachine = new StateMachine<DialogueEventState, DialogueEventTrigger>(initialState);
        questStateMachine.Configure(initialState)
            .Permit(new DialogueEventTrigger("stage_1_ignore"), new DialogueEventState("complete"))
            .Permit(new DialogueEventTrigger("stage_1_fast"), new DialogueEventState("stage_2_fast"))
            .Permit(new DialogueEventTrigger("stage_1_help"), new DialogueEventState("stage_2_help"));

        questStateMachine.Configure(new DialogueEventState("stage_2_fast"))
            .Permit(new DialogueEventTrigger("stage_2_complete"), new DialogueEventState("complete"));

        questStateMachine.Configure(new DialogueEventState("stage_2_help"))
            .Permit(new DialogueEventTrigger("stage_2_complete"), new DialogueEventState("stage_3"));

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [initialState] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            },
            [new DialogueEventState("stage_2_fast")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new StoryKeyRequirement("synth_as_parent_stage_2_fast")
            },
            [new DialogueEventState("stage_2_help")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new StoryKeyRequirement("synth_as_parent_stage_2_help")
            },
            [new DialogueEventState("stage_3")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new StoryKeyRequirement("synth_as_parent_stage_3")
            }
        };

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [initialState] = "synth_as_parent_stage_1",
            [new DialogueEventState("stage_2_fast")] = "synth_as_parent_stage_2_fast",
            [new DialogueEventState("stage_2_help")] = "synth_as_parent_stage_2",
            [new DialogueEventState("stage_3")] = "synth_as_parent_stage_3"
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
                new Job(
                    new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.Defeats, new JobGoalValue(12)),
                    nameof(UiResource.DefeatsJobTitleSid),
                    nameof(UiResource.DefeatsJobProgressPatternSid),
                    nameof(UiResource.DefeatsJobCompletePatternSid))
            },
            Aftermaths = new IStoryPointAftermath[]
            {
                new TriggerQuestStoryPointAftermath("synth_as_parent", new DialogueEventTrigger("stage_1_complete"),
                    services.EventCatalog)
            }
        };

        spList.Add(synthStage1Story);

        return spList;
    }
}