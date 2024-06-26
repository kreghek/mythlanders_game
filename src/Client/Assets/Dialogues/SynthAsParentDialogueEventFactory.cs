﻿using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Assets.DialogueEventRequirements;
using Client.Assets.StoryPointAftermaths;
using Client.Assets.StoryPointJobs;
using Client.Core;

using CombatDicesTeam.Dialogues;

using JetBrains.Annotations;

using Stateless;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class SynthAsParentDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        // Canonical story
        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);

        questStateMachine.Configure(DialogueConstants.InitialStage)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage1_Ignore_Trigger, DialogueConstants.CompleteStage)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage1_Extradite_Trigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage1_Fast_In_Progress)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage1_Repair_Trigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage1_Canon_In_Progress);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage1_Canon_In_Progress)
            .Permit(DialogueConstants.CompleteCurrentStageChallengeTrigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage2_Canon);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage2_Canon)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage2_Continue_Trigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage2_Canon_In_Progress);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage2_Canon_In_Progress)
            .Permit(DialogueConstants.CompleteCurrentStageChallengeTrigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage3_Canon);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage3_Canon)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage3_Continue_Trigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage3_Canon_In_Progress);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage3_Canon_In_Progress)
            .Permit(DialogueConstants.CompleteCurrentStageChallengeTrigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage4_Canon);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage4_Canon)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage4_Continue_Trigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage4_Canon_In_Progress);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage4_Canon_In_Progress)
            .Permit(DialogueConstants.CompleteCurrentStageChallengeTrigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage5_Canon);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage5_Canon)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage5_Lead_Trigger, DialogueConstants.CompleteStage);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage5_Canon)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage5_Leave_Trigger, DialogueConstants.CompleteStage);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage1_Fast_In_Progress)
            .Permit(DialogueConstants.CompleteCurrentStageChallengeTrigger,
                DialogueConstants.SideQuests.SynthAsParent.Stage2_Fast);

        questStateMachine.Configure(DialogueConstants.SideQuests.SynthAsParent.Stage2_Fast)
            .Permit(DialogueConstants.SideQuests.SynthAsParent.Stage2_Continue_Trigger,
                DialogueConstants.CompleteStage);

        static string GetDialogueFileName(string stageName)
        {
            var sid = DialogueConstants.SideQuests.SynthAsParent.Sid;
            return $"{sid}_{stageName}";
        }

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] = GetDialogueFileName("stage_1"),
            [DialogueConstants.SideQuests.SynthAsParent.Stage2_Canon] = GetDialogueFileName("stage_2"),
            [DialogueConstants.SideQuests.SynthAsParent.Stage3_Canon] = GetDialogueFileName("stage_3"),
            [DialogueConstants.SideQuests.SynthAsParent.Stage4_Canon] = GetDialogueFileName("stage_4"),
            [DialogueConstants.SideQuests.SynthAsParent.Stage5_Canon] = GetDialogueFileName("stage_5"),

            [DialogueConstants.SideQuests.SynthAsParent.Stage2_Fast] = GetDialogueFileName("stage_2_fast")
        };

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSids.Desert),
                new NoSideQuestRequirement()
            },
            [DialogueConstants.SideQuests.SynthAsParent.Stage2_Canon] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSids.Desert)
            },
            [DialogueConstants.SideQuests.SynthAsParent.Stage3_Canon] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSids.Desert)
            },
            [DialogueConstants.SideQuests.SynthAsParent.Stage4_Canon] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSids.Desert)
            },
            [DialogueConstants.SideQuests.SynthAsParent.Stage5_Canon] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSids.Desert)
            },
            [DialogueConstants.SideQuests.SynthAsParent.Stage2_Fast] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSids.Desert)
            }
        };

        return new DialogueEvent(DialogueConstants.SideQuests.SynthAsParent.Sid, questStateMachine, requirements,
            dialogues);
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        var spList = new List<IStoryPoint>();

        var synthStage1Task = new StoryPoint($"{DialogueConstants.SideQuests.SynthAsParent.Sid}_stage_1")
        {
            TitleSid = DialogueConstants.SideQuests.SynthAsParent.Sid,
            CurrentJobs = new[]
            {
                new Job(
                    new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.Defeats, new JobGoalValue(12)),
                    nameof(GameObjectResources.DefeatsJobTitleSid),
                    nameof(GameObjectResources.DefeatsJobProgressPatternSid),
                    nameof(GameObjectResources.DefeatsJobCompletePatternSid))
            },
            Aftermaths = new IStoryPointAftermath[]
            {
                new TriggerQuestStoryPointAftermath(DialogueConstants.SideQuests.SynthAsParent.Sid,
                    DialogueConstants.CompleteCurrentStageChallengeTrigger,
                    services.EventCatalog)
            }
        };
        spList.Add(synthStage1Task);

        var synthStage2Task = new StoryPoint($"{DialogueConstants.SideQuests.SynthAsParent.Sid}_stage_2")
        {
            TitleSid = DialogueConstants.SideQuests.SynthAsParent.Sid,
            CurrentJobs = new[]
            {
                new Job(
                    new JobScheme(JobScopeCatalog.Global, JobTypeCatalog.WinCampaigns, new JobGoalValue(1)),
                    nameof(GameObjectResources.CampaignsJobTitleSid),
                    nameof(GameObjectResources.CampaignsJobProgressPatternSid),
                    nameof(GameObjectResources.CampaignsJobCompletePatternSid))
            },
            Aftermaths = new IStoryPointAftermath[]
            {
                new TriggerQuestStoryPointAftermath(DialogueConstants.SideQuests.SynthAsParent.Sid,
                    DialogueConstants.CompleteCurrentStageChallengeTrigger,
                    services.EventCatalog)
            }
        };
        spList.Add(synthStage2Task);

        return spList;
    }
}