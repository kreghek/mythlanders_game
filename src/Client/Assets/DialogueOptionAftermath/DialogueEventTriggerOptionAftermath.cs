using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class DialogueEventTriggerOptionAftermath : IDialogueOptionAftermath<AftermathContext>
{
    private readonly string _trigger;

    public DialogueEventTriggerOptionAftermath(string trigger)
    {
        _trigger = trigger;
    }

    public void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.CurrentDialogueEvent.Trigger(new DialogueEventTrigger(_trigger));
    }
}