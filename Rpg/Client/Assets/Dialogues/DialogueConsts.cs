using Client.Core.Dialogues;

namespace Client.Assets.Dialogues;

internal static class DialogueConsts
{
    public static DialogueEventState InitialStage { get; } = new DialogueEventState("stage_1");
    public static DialogueEventState CompleteStage { get; } = new DialogueEventState("complete");

    public static DialogueEventTrigger CompleteCurrentStageChallangeTrigger { get; } = new DialogueEventTrigger("complete_challange");

    public static class SideQuests
    {
        public static class SynthAsParent
        {
            public static DialogueEventState Stage1_Canon_In_Progress { get; } = new DialogueEventState("stage_1_canon_in_progress") { NoDialogue = true };
            public static DialogueEventState Stage2_Canon { get; } = new DialogueEventState("stage_2_canon");
            public static DialogueEventState Stage2_Canon_In_Progress { get; } = new DialogueEventState("stage_2_canon_in_progress") { NoDialogue = true };
            public static DialogueEventState Stage3_Canon { get; } = new DialogueEventState("stage_3_canon");
            public static DialogueEventState Stage3_Canon_In_Progress { get; } = new DialogueEventState("stage_3_canon_in_progress") { NoDialogue = true };
            public static DialogueEventState Stage4_Canon { get; } = new DialogueEventState("stage_4_canon");
            public static DialogueEventState Stage4_Canon_In_Progress { get; } = new DialogueEventState("stage_4_canon_in_progress") { NoDialogue = true };
            public static DialogueEventState Stage5_Canon { get; } = new DialogueEventState("stage_5_canon");
            public static DialogueEventState Stage5_Canon_In_Progress { get; } = new DialogueEventState("stage_5_canon_in_progress") { NoDialogue = true };

            public static DialogueEventState Stage1_Fast_In_Progress { get; } = new DialogueEventState("stage_1_fast_in_progress") { NoDialogue = true };
            public static DialogueEventState Stage2_Fast { get; } = new DialogueEventState("stage_2_fast");

            public static DialogueEventTrigger Stage1_Ignore_Trigger { get; } = new DialogueEventTrigger("stage_1_ignore");
            public static DialogueEventTrigger Stage1_Fast_Trigger { get; } = new DialogueEventTrigger("stage_1_fast");
            public static DialogueEventTrigger Stage1_Repair_Trigger { get; } = new DialogueEventTrigger("stage_1_repair");

            public static DialogueEventTrigger Stage2_Continue_Trigger { get; } = new DialogueEventTrigger("stage_2_continue");
            public static DialogueEventTrigger Stage3_Continue_Trigger { get; } = new DialogueEventTrigger("stage_3_continue");
            public static DialogueEventTrigger Stage4_Continue_Trigger { get; } = new DialogueEventTrigger("stage_4_continue");
            public static DialogueEventTrigger Stage5_Continue_Trigger { get; } = new DialogueEventTrigger("stage_5_continue");
        }
    }
}
