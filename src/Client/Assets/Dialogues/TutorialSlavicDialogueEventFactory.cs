using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;
using Client.Core;

using CombatDicesTeam.Dialogues;

using JetBrains.Annotations;

using Stateless;

namespace Client.Assets.Dialogues;

[UsedImplicitly]
internal sealed class TutorialSlavicDialogueEventFactory : IDialogueEventFactory
{
    public DialogueEvent CreateEvent(IDialogueEventFactoryServices services)
    {
        var questStateMachine =
            new StateMachine<DialogueEventState, DialogueEventTrigger>(DialogueConstants.InitialStage);
        
        var requirements = new Dictionary<DialogueEventState, IReadOnlyCollection<IDialogueEventRequirement>>
        {
            [DialogueConstants.InitialStage] = Array.Empty<IDialogueEventRequirement>()
        };
        
        var dialogues = new Dictionary<DialogueEventState, string>
        {
            [DialogueConstants.InitialStage] = GetDialogueFileName(DialogueConstants.MainPlot.SlavicTutorial.Stage1)
        };

        return new DialogueEvent(DialogueConstants.MainPlot.SlavicTutorial.Sid, questStateMachine, requirements,
            dialogues);
        
        static string GetDialogueFileName(string stageName)
        {
            var sid = DialogueConstants.MainPlot.SlavicTutorial.Sid;
            return $"{sid}_{stageName}";
        }
    }

    public IReadOnlyCollection<IStoryPoint> CreateStoryPoints(IDialogueEventFactoryServices services)
    {
        return ArraySegment<IStoryPoint>.Empty;
    }
}