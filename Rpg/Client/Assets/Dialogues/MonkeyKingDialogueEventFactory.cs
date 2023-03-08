using System.Collections.Generic;

using Client.Assets.DialogueEventRequirements;
using Client.Assets.StoryPointAftermaths;
using Client.Assets.StoryPointJobs;
using Client.Core;
using Client.Core.Dialogues;

using JetBrains.Annotations;

using Rpg.Client.Core;

using Stateless;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class MonkeyKingDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        // Canonical story
        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);

        questStateMachine.Configure(DialogueConstants.InitialStage)
            .Permit(DialogueConstants.SideQuests.MonkeyKing.Stage1_Ignore_Trigger, DialogueConstants.CompleteStage)
            .Permit(DialogueConstants.SideQuests.MonkeyKing.Stage1_Help_Trigger,
                DialogueConstants.SideQuests.MonkeyKing.Stage1_Canon_In_Progress);

        questStateMachine.Configure(DialogueConstants.SideQuests.MonkeyKing.Stage1_Canon_In_Progress)
            .Permit(DialogueConstants.CompleteCurrentStageChallengeTrigger, DialogueConstants.CompleteStage);

        static string GetDialogueFileName(string stageName)
        {
            var sid = DialogueConstants.SideQuests.MonkeyKing.Sid;
            return $"{sid}_{stageName}";
        }

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] = GetDialogueFileName("stage_1"),
        };

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = new IDialogueEventRequirement[]
            {
                new LocationRequirement(LocationSid.Monastery),
                new NoSideQuestRequirement()
            }
        };

        return new DialogueEvent(DialogueConstants.SideQuests.MonkeyKing.Sid, questStateMachine, requirements,
            dialogues);
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        var spList = new List<IStoryPoint>();

        var synthStage1Task = new StoryPoint($"{DialogueConstants.SideQuests.MonkeyKing.Sid}_stage_1")
        {
            TitleSid = DialogueConstants.SideQuests.SynthAsParent.Sid,
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
                new TriggerQuestStoryPointAftermath(DialogueConstants.SideQuests.MonkeyKing.Sid,
                    DialogueConstants.CompleteCurrentStageChallengeTrigger,
                    services.EventCatalog)
            }
        };
        spList.Add(synthStage1Task);

        return spList;
    }
}