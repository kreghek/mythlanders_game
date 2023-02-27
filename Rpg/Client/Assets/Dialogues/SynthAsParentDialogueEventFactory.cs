using System.Collections.Generic;

using Client.Assets.DialogueEventRequirements;
using Client.Assets.StoryPointAftermaths;
using Client.Core;
using Client.Core.Dialogues;

using JetBrains.Annotations;

using Rpg.Client.Assets.StoryPointJobs;
using Rpg.Client.Core;

using Stateless;

namespace Client.Assets.Dialogues;

internal static class DialogueConsts
{
    public static DialogueEventState InitialStage { get; } = new DialogueEventState("stage_1");
    public static DialogueEventState CompleteStage { get; } = new DialogueEventState("complete");

    public static DialogueEventTrigger CompleteStageChallangeTrigger { get; } = new DialogueEventTrigger("complete_challange");

    public static class SideQuests
    {
        public static class SynthAsParent
        {
            public static DialogueEventState State1_Canon { get; } = new DialogueEventState("stage_1_canon");
            public static DialogueEventState State2_Canon { get; } = new DialogueEventState("stage_2_canon");
            public static DialogueEventState State3_Canon { get; } = new DialogueEventState("stage_3_canon");

            public static DialogueEventState State1_Fast { get; } = new DialogueEventState("stage_1_fast");
            public static DialogueEventState State2_Fast { get; } = new DialogueEventState("stage_2_fast");

            public static DialogueEventTrigger Stage1_Ignore_Trigger { get; } = new DialogueEventTrigger("stage_1_ignore");
            public static DialogueEventTrigger Stage1_Fast_Trigger { get; } = new DialogueEventTrigger("stage_1_fast");
            public static DialogueEventTrigger Stage1_Help_Trigger { get; } = new DialogueEventTrigger("stage_1_help");
        }
    }
}

[UsedImplicitly]
internal sealed class SynthAsParentDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        // Canoniacal story
        var questStateMachine = new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConsts.InitialStage);
        questStateMachine.Configure(DialogueConsts.InitialStage)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage1_Ignore_Trigger, DialogueConsts.CompleteStage)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage1_Fast_Trigger, DialogueConsts.SideQuests.SynthAsParent.State1_Fast)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage1_Help_Trigger, DialogueConsts.SideQuests.SynthAsParent.State1_Canon);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.State1_Canon)
            .Permit(DialogueConsts.CompleteStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.State2_Canon);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.State2_Canon)
            .Permit(DialogueConsts.CompleteStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.State3_Canon);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.State3_Canon)
            .Permit(DialogueConsts.CompleteStageChallangeTrigger, DialogueConsts.CompleteStage);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.State1_Fast)
            .Permit(DialogueConsts.CompleteStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.State2_Fast);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.State2_Fast)
            .Permit(DialogueConsts.CompleteStageChallangeTrigger, DialogueConsts.CompleteStage);

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConsts.InitialStage] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan),
                new NoSideQuestRequirement()
            },
            [DialogueConsts.SideQuests.SynthAsParent.State2_Canon] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new StoryKeyRequirement("synth_as_parent_stage_2_help"),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            },
            [new DialogueEventState("stage_2_fast")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new StoryKeyRequirement("synth_as_parent_stage_2_fast"),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            },
            [new DialogueEventState("stage_3")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new StoryKeyRequirement("synth_as_parent_stage_3"),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            }
        };

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConsts.InitialStage] = "synth_as_parent_stage_1",
            [DialogueConsts.SideQuests.SynthAsParent.State2_Canon] = "synth_as_parent_stage_2",
            [DialogueConsts.SideQuests.SynthAsParent.State3_Canon] = "synth_as_parent_stage_3",

            [DialogueConsts.SideQuests.SynthAsParent.State2_Fast] = "synth_as_parent_stage_2_fast",
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
                new TriggerQuestStoryPointAftermath("synth_as_parent", DialogueConsts.CompleteStageChallangeTrigger,
                    services.EventCatalog)
            }
        };

        spList.Add(synthStage1Story);

        return spList;
    }
}