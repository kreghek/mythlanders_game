using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens;

using CombatDicesTeam.Dialogues;

namespace Client.ScreenManagement.Ui.TextEvents;

internal abstract record TextEventScreenArgsBase(
    HeroCampaign Campaign,
    Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue,
    DialogueEvent DialogueEvent) : CampaignScreenTransitionArgumentsBase(Campaign);