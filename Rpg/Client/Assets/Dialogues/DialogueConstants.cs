using System.Diagnostics.CodeAnalysis;

using Client.Core.Dialogues;

namespace Client.Assets.Dialogues;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class DialogueConstants
{
    public static DialogueEventState InitialStage { get; } = new("stage_1");
    public static DialogueEventState CompleteStage { get; } = new("complete");

    public static DialogueEventTrigger CompleteCurrentStageChallengeTrigger { get; } = new("complete_challenge");

    public static class SideQuests
    {
        public static class SynthAsParent
        {
            public static string Sid { get; } = "synth_as_parent";

            public static DialogueEventState Stage1_Canon_In_Progress { get; } =
                new("stage_1_canon_in_progress") { NoDialogue = true };

            public static DialogueEventState Stage2_Canon { get; } = new("stage_2_canon");

            public static DialogueEventState Stage2_Canon_In_Progress { get; } =
                new("stage_2_canon_in_progress") { NoDialogue = true };

            public static DialogueEventState Stage3_Canon { get; } = new("stage_3_canon");

            public static DialogueEventState Stage3_Canon_In_Progress { get; } =
                new("stage_3_canon_in_progress") { NoDialogue = true };

            public static DialogueEventState Stage4_Canon { get; } = new("stage_4_canon");

            public static DialogueEventState Stage4_Canon_In_Progress { get; } =
                new("stage_4_canon_in_progress") { NoDialogue = true };

            public static DialogueEventState Stage5_Canon { get; } = new("stage_5_canon");

            public static DialogueEventState Stage1_Fast_In_Progress { get; } =
                new("stage_1_fast_in_progress") { NoDialogue = true };

            public static DialogueEventState Stage2_Fast { get; } = new("stage_2_fast");

            public static DialogueEventTrigger Stage1_Ignore_Trigger { get; } = new("stage_1_ignore");
            public static DialogueEventTrigger Stage1_Fast_Trigger { get; } = new("stage_1_fast");
            public static DialogueEventTrigger Stage1_Repair_Trigger { get; } = new("stage_1_repair");

            public static DialogueEventTrigger Stage2_Continue_Trigger { get; } = new("stage_2_continue");
            public static DialogueEventTrigger Stage3_Continue_Trigger { get; } = new("stage_3_continue");
            public static DialogueEventTrigger Stage4_Continue_Trigger { get; } = new("stage_4_continue");
            public static DialogueEventTrigger Stage5_Lead_Trigger { get; } = new("stage_5_lead");
            public static DialogueEventTrigger Stage5_Leave_Trigger { get; } = new("stage_5_leave");
        }
    }
}