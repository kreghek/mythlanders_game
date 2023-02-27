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

[UsedImplicitly]
internal sealed class SynthAsParentDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        // Canoniacal story
        var questStateMachine = new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConsts.InitialStage);
        questStateMachine.Configure(DialogueConsts.InitialStage)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage1_Ignore_Trigger, DialogueConsts.CompleteStage)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage1_Fast_Trigger, DialogueConsts.SideQuests.SynthAsParent.Stage1_Fast_In_Progress)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage1_Repair_Trigger, DialogueConsts.SideQuests.SynthAsParent.Stage1_Canon_In_Progress);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage1_Canon_In_Progress)
            .Permit(DialogueConsts.CompleteCurrentStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.Stage2_Canon);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage2_Canon)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage2_Continue_Trigger, DialogueConsts.SideQuests.SynthAsParent.Stage2_Canon_In_Progress);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage2_Canon_In_Progress)
            .Permit(DialogueConsts.CompleteCurrentStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.Stage3_Canon);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage3_Canon)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage3_Continue_Trigger, DialogueConsts.SideQuests.SynthAsParent.Stage3_Canon_In_Progress);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage3_Canon_In_Progress)
            .Permit(DialogueConsts.CompleteCurrentStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.Stage4_Canon);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage4_Canon)
            .Permit(DialogueConsts.SideQuests.SynthAsParent.Stage4_Continue_Trigger, DialogueConsts.SideQuests.SynthAsParent.Stage4_Canon_In_Progress);

        questStateMachine.Configure(DialogueConsts.SideQuests.SynthAsParent.Stage4_Canon_In_Progress)
            .Permit(DialogueConsts.CompleteCurrentStageChallangeTrigger, DialogueConsts.SideQuests.SynthAsParent.Stage5_Canon);

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConsts.InitialStage] = "synth_as_parent_stage_1",
            [DialogueConsts.SideQuests.SynthAsParent.Stage2_Canon] = "synth_as_parent_stage_2",
            [DialogueConsts.SideQuests.SynthAsParent.Stage3_Canon] = "synth_as_parent_stage_3",

            [DialogueConsts.SideQuests.SynthAsParent.Stage2_Fast] = "synth_as_parent_stage_2_fast",
        };


        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConsts.InitialStage] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan),
                new NoSideQuestRequirement()
            },
            [DialogueConsts.SideQuests.SynthAsParent.Stage2_Canon] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            },
            [new DialogueEventState("stage_2_fast")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            },
            [new DialogueEventState("stage_3")] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Desert),
                new HeroInPartyRequirement(UnitName.Swordsman, UnitName.Partisan)
            }
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
                new TriggerQuestStoryPointAftermath("synth_as_parent", DialogueConsts.CompleteCurrentStageChallangeTrigger,
                    services.EventCatalog)
            }
        };

        spList.Add(synthStage1Story);

        return spList;
    }
}