using Client.Assets.Catalogs.Dialogues;
using Client.Core.Campaigns;
using Client.ScreenManagement.Ui.TextEvents;

using CombatDicesTeam.Dialogues;

using Core.Crises;

namespace Client.GameScreens.Crisis;

internal sealed record CrisisScreenTransitionArguments
    (HeroCampaign Campaign, EventType EventType,
        Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue,
        DialogueEvent DialogueEvent) : TextEventScreenArgsBase(Campaign, CurrentDialogue, DialogueEvent);