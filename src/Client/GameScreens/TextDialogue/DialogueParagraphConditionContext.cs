using System.Collections.Generic;

using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.TextDialogue;

internal sealed class DialogueParagraphConditionContext : IDialogueParagraphConditionContext
{
    private readonly IEventContext _eventContext;

    public DialogueParagraphConditionContext(IEventContext eventContext)
    {
        _eventContext = eventContext;
    }

    public IReadOnlyCollection<string> CurrentHeroes => _eventContext.CurrentHeroes;
}