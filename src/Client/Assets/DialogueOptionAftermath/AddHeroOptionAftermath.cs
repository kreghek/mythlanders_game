﻿using Client.Core;
using Client.Core.Dialogues;
using Client.Core.Heroes;

namespace Client.Assets.DialogueOptionAftermath;

internal sealed class AddHeroOptionAftermath : IDialogueOptionAftermath
{
    private readonly UnitScheme _scheme;

    public AddHeroOptionAftermath(UnitScheme scheme)
    {
        _scheme = scheme;
    }

    public void Apply(IEventContext dialogContext)
    {
        const int DEFAULT_LEVEL = 1;
        var unit = new Hero(_scheme, DEFAULT_LEVEL)
        {
            IsPlayerControlled = true
        };
        dialogContext.AddNewCharacter(unit);
    }
}