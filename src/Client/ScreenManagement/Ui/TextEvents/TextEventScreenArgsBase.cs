using Client.Assets.Catalogs.Dialogues;
using Client.Core.Campaigns;
using Client.GameScreens;

using CombatDicesTeam.Dialogues;

namespace Client.ScreenManagement.Ui.TextEvents;

internal abstract record CampaignTextEventScreenArgsBase(HeroCampaign Campaign, Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue, DialogueEvent DialogueEvent): TextEventScreenArgsBase(CurrentDialogue)

internal abstract record TextEventScreenArgsBase(
    Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue) : IScreenTransitionArguments;