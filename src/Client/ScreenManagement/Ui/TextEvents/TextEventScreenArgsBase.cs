using Client.Assets.Catalogs.Dialogues;
using Client.Core.Campaigns;

using CombatDicesTeam.Dialogues;

namespace Client.ScreenManagement.Ui.TextEvents;

internal abstract record CampaignTextEventScreenArgsBase(HeroCampaign Campaign,
    Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue, DialogueEvent DialogueEvent) :
    TextEventScreenArgsBase<ParagraphConditionContext, CampaignAftermathContext>(CurrentDialogue);

internal abstract record TextEventScreenArgsBase<TParagraphConditionContext, TAftermathContext>(
    Dialogue<TParagraphConditionContext, TAftermathContext> CurrentDialogue) : IScreenTransitionArguments;