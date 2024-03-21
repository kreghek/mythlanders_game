using Client.Assets.Catalogs.Dialogues;
using Client.Core.Campaigns;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;

using Core.Crises;

namespace Client.GameScreens.PreHistory;

internal sealed record PreHistoryScreenScreenTransitionArguments
(Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue,
    ) : TextEventScreenArgsBase(Campaign, CurrentDialogue, DialogueEvent);