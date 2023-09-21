using System;
using System.Collections.Generic;

using Client.Assets.Catalogs.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal class DialogueEventTriggerOptionAftermath : DialogueOptionAftermathBase
{
    private readonly string _trigger;

    public DialogueEventTriggerOptionAftermath(string trigger)
    {
        _trigger = trigger;

        IsHidden = true;
    }

    public override void Apply(AftermathContext aftermathContext)
    {
        aftermathContext.CurrentDialogueEvent.Trigger(new DialogueEventTrigger(_trigger));
    }

    protected override IReadOnlyList<string> GetDescriptionValues(AftermathContext aftermathContext)
    {
        return ArraySegment<string>.Empty;
    }
}