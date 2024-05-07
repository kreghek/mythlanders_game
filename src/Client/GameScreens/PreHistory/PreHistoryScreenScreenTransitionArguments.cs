using Client.Assets.Catalogs.Dialogues;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.PreHistory;

internal sealed record PreHistoryScreenScreenTransitionArguments
(Dialogue<PreHistoryConditionContext, PreHistoryAftermathContext> CurrentDialogue
) : TextEventScreenArgsBase<PreHistoryConditionContext, PreHistoryAftermathContext>(CurrentDialogue);