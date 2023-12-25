using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.TextDialogue;

internal sealed record TextDialogueScreenTransitionArgs(
    HeroCampaign Campaign,
    Dialogue<ParagraphConditionContext, CampaignAftermathContext> CurrentDialogue,
    DialogueEvent DialogueEvent,
    ILocationSid Location) : CampaignScreenTransitionArgumentsBase(Campaign);