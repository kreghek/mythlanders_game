﻿using Client.Assets.GlobalEffects;
using Client.Core;
using Client.Core.Dialogues;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class UnitDeepPreyingOptionAftermath : IDialogueOptionAftermath
{
    private readonly UnitName _name;

    public UnitDeepPreyingOptionAftermath(UnitName name)
    {
        _name = name;
    }

    public void Apply(IEventContext dialogContext)
    {
        var globalEvent = new CharacterDeepPreyingGlobeEvent(_name);

        dialogContext.AddNewGlobalEvent(globalEvent);
    }
}