using Rpg.Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class DialogueEventTriggerOptionAftermath : IDialogueOptionAftermath
{
    private readonly string _trigger;

    public DialogueEventTriggerOptionAftermath(string trigger)
    {
        _trigger = trigger;
    }

    public void Apply(IEventContext dialogContext)
    {
        dialogContext.CurrentDualogueEvent.Trigger(new Core.Dialogues.DialogueEventTrigger(_trigger));
    }
}