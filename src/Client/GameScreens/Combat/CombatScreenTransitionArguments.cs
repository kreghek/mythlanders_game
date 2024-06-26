using Client.Assets.Catalogs.Dialogues;
using Client.Core;
using Client.Core.Campaigns;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.Combat;

internal sealed record CombatScreenTransitionArguments(HeroCampaign Campaign,
    CombatSequence CombatSequence,
    int CurrentCombatIndex,
    bool IsFreeCombat,
    ILocationSid Location,
    Dialogue<ParagraphConditionContext, CampaignAftermathContext>? VictoryDialogue) :
    CampaignScreenTransitionArgumentsBase(Campaign);