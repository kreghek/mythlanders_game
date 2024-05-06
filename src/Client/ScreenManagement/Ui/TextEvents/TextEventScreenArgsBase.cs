using CombatDicesTeam.Dialogues;

namespace Client.ScreenManagement.Ui.TextEvents;

internal abstract record TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext>(
    Dialogue<TParagraphConditionContext, TAftermathContext> CurrentDialogue) : IScreenTransitionArguments;