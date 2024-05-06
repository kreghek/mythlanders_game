using Client.Assets.Catalogs.Dialogues;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed record PreHistoryScreenScreenTransitionArguments
(Dialogue<ParagraphConditionContext, PreHistoryAftermathContext> CurrentDialogue
) : TextEventScreenArgsBase<ParagraphConditionContext, PreHistoryAftermathContext>(CurrentDialogue);