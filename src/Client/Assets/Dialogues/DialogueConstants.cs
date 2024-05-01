using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.Dialogues;

internal static class DialogueConstants
{
    public static DialogueEventTrigger CompleteCurrentStageChallengeTrigger { get; } = new("complete_challenge");
    public static DialogueEventState CompleteStage { get; } = new("complete");
    public static DialogueEventState InitialStage { get; } = new("stage_1");

    public static class MainPlot
    {
        public static class SlavicTutorial
        {
            public static string Sid => "slavic_tutorial";
            public static DialogueEventTrigger Stage1_Fight_Trigger { get; } = new("stage_1_fight");

            public static string Stage1Dialogue => "stage_1";
            public static DialogueEventState Stage2 { get; } = new("stage_2") { NoDialogue = true };
            public static DialogueEventTrigger Stage2_Meet_Heroes_Trigger { get; } = new("stage_2_meet_heroes");


            public static string Stage2Dialogue => "stage_2";

            public static DialogueEventState Stage3 { get; } = new("stage_3") { NoDialogue = true };
            public static DialogueEventTrigger Stage3_WaySelection_Trigger { get; } = new("stage_3_way_selection");
            public static string Stage3Dialogue => "stage_3";

            public static DialogueEventState Stage4 { get; } = new("stage_4") { NoDialogue = true };
            public static string Stage4Dialogue => "stage_4";
        }

        public static class ChineseTutorial
        {
            public static string Sid => "chinese_tutorial";
            public static DialogueEventTrigger Stage1_Fight_Trigger { get; } = new("stage_1_fight");

            public static string Stage1Dialogue => "stage_1";
            public static DialogueEventState Stage2 { get; } = new("stage_2") { NoDialogue = true };
            public static DialogueEventTrigger Stage2_Meet_Heroes_Trigger { get; } = new("stage_2_meet_heroes");


            public static string Stage2Dialogue => "stage_2";

            public static DialogueEventState Stage3 { get; } = new("stage_3") { NoDialogue = true };
            public static DialogueEventTrigger Stage3_WaySelection_Trigger { get; } = new("stage_3_way_selection");
            public static string Stage3Dialogue => "stage_3";

            public static DialogueEventState Stage4 { get; } = new("stage_4") { NoDialogue = true };
            public static string Stage4Dialogue => "stage_4";
        }

        public static class GreekTutorial
        {
            public static string Sid => "greek_tutorial";
            public static DialogueEventTrigger Stage1_Fight_Trigger { get; } = new("stage_1_fight");

            public static string Stage1Dialogue => "stage_1";
            public static DialogueEventState Stage2 { get; } = new("stage_2") { NoDialogue = true };
            public static DialogueEventTrigger Stage2_Meet_Heroes_Trigger { get; } = new("stage_2_meet_heroes");


            public static string Stage2Dialogue => "stage_2";

            public static DialogueEventState Stage3 { get; } = new("stage_3") { NoDialogue = true };
            public static DialogueEventTrigger Stage3_WaySelection_Trigger { get; } = new("stage_3_way_selection");
            public static string Stage3Dialogue => "stage_3";

            public static DialogueEventState Stage4 { get; } = new("stage_4") { NoDialogue = true };
            public static string Stage4Dialogue => "stage_4";
        }
        
        public static class EgyptTutorial
        {
            public static string Sid => "egypt_tutorial";
            public static DialogueEventTrigger Stage1_Fight_Trigger { get; } = new("stage_1_fight");

            public static string Stage1Dialogue => "stage_1";
            public static DialogueEventState Stage2 { get; } = new("stage_2") { NoDialogue = true };
            public static DialogueEventTrigger Stage2_Meet_Heroes_Trigger { get; } = new("stage_2_meet_heroes");


            public static string Stage2Dialogue => "stage_2";

            public static DialogueEventState Stage3 { get; } = new("stage_3") { NoDialogue = true };
            public static DialogueEventTrigger Stage3_WaySelection_Trigger { get; } = new("stage_3_way_selection");
            public static string Stage3Dialogue => "stage_3";

            public static DialogueEventState Stage4 { get; } = new("stage_4") { NoDialogue = true };
            public static string Stage4Dialogue => "stage_4";
        }
        
        public static class Episode1
        {
            public static string Sid => "main_plot_e1";

            public static string Stage1Dialogue => "stage_1";
        }
    }

    public static class SideQuests
    {
        public static class SynthAsParent
        {
            public static string Sid => "synth_as_parent";

            public static DialogueEventState Stage1_Canon_In_Progress { get; } =
                new("stage_1_canon_in_progress") { NoDialogue = true };

            public static DialogueEventTrigger Stage1_Extradite_Trigger { get; } = new("stage_1_extradite");

            public static DialogueEventState Stage1_Fast_In_Progress { get; } =
                new("stage_1_fast_in_progress") { NoDialogue = true };

            public static DialogueEventTrigger Stage1_Ignore_Trigger { get; } = new("stage_1_ignore");
            public static DialogueEventTrigger Stage1_Repair_Trigger { get; } = new("stage_1_repair");

            public static DialogueEventState Stage2_Canon { get; } = new("stage_2_canon");

            public static DialogueEventState Stage2_Canon_In_Progress { get; } =
                new("stage_2_canon_in_progress") { NoDialogue = true };

            public static DialogueEventTrigger Stage2_Continue_Trigger { get; } = new("stage_2_continue");

            public static DialogueEventState Stage2_Fast { get; } = new("stage_2_fast");

            public static DialogueEventState Stage3_Canon { get; } = new("stage_3_canon");

            public static DialogueEventState Stage3_Canon_In_Progress { get; } =
                new("stage_3_canon_in_progress") { NoDialogue = true };

            public static DialogueEventTrigger Stage3_Continue_Trigger { get; } = new("stage_3_continue");

            public static DialogueEventState Stage4_Canon { get; } = new("stage_4_canon");

            public static DialogueEventState Stage4_Canon_In_Progress { get; } =
                new("stage_4_canon_in_progress") { NoDialogue = true };

            public static DialogueEventTrigger Stage4_Continue_Trigger { get; } = new("stage_4_continue");

            public static DialogueEventState Stage5_Canon { get; } = new("stage_5_canon");
            public static DialogueEventTrigger Stage5_Lead_Trigger { get; } = new("stage_5_lead");
            public static DialogueEventTrigger Stage5_Leave_Trigger { get; } = new("stage_5_leave");
        }

        public static class MonkeyKing
        {
            public static string Sid { get; } = "monkey_king";

            public static DialogueEventState Stage1_Canon_In_Progress { get; } =
                new("stage_1_canon_in_progress") { NoDialogue = true };

            public static DialogueEventTrigger Stage1_Help_Trigger { get; } = new("stage_1_help");

            public static DialogueEventTrigger Stage1_Ignore_Trigger { get; } = new("stage_1_ignore");
        }
    }

    public static class SmallEvents
    {
        public static DialogueEventState Stage1_Canon_In_Progress { get; } =
            new("stage_1_canon_in_progress") { NoDialogue = true };
    }
}