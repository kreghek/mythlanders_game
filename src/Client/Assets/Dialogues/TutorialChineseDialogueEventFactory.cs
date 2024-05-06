using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

using JetBrains.Annotations;

using Stateless;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class TutorialChineseDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);

        questStateMachine.Configure(DialogueConstants.InitialStage)
            .Permit(DialogueConstants.MainPlot.ChineseTutorial.Stage1_Fight_Trigger,
                DialogueConstants.MainPlot.ChineseTutorial.Stage2);
        questStateMachine.Configure(DialogueConstants.MainPlot.ChineseTutorial.Stage2)
            .Permit(DialogueConstants.MainPlot.ChineseTutorial.Stage2_Meet_Heroes_Trigger,
                DialogueConstants.MainPlot.ChineseTutorial.Stage3);
        questStateMachine.Configure(DialogueConstants.MainPlot.ChineseTutorial.Stage3)
            .Permit(DialogueConstants.MainPlot.ChineseTutorial.Stage3_WaySelection_Trigger,
                DialogueConstants.MainPlot.ChineseTutorial.Stage4);

        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = Array.Empty<IDialogueEventRequirement>()
        };

        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] =
                GetDialogueFileName(DialogueConstants.MainPlot.ChineseTutorial.Stage1Dialogue),
            [DialogueConstants.MainPlot.ChineseTutorial.Stage2] =
                GetDialogueFileName(DialogueConstants.MainPlot.ChineseTutorial.Stage2Dialogue),
            [DialogueConstants.MainPlot.ChineseTutorial.Stage3] =
                GetDialogueFileName(DialogueConstants.MainPlot.ChineseTutorial.Stage3Dialogue),
            [DialogueConstants.MainPlot.ChineseTutorial.Stage4] =
                GetDialogueFileName(DialogueConstants.MainPlot.ChineseTutorial.Stage4Dialogue)
        };

        return new DialogueEvent(DialogueConstants.MainPlot.ChineseTutorial.Sid, questStateMachine, requirements,
            dialogues);

        static string GetDialogueFileName(string stageName)
        {
            var sid = DialogueConstants.MainPlot.ChineseTutorial.Sid;
            return $"{sid}_{stageName}";
        }
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        return ArraySegment<IStoryPoint>.Empty;
    }
}