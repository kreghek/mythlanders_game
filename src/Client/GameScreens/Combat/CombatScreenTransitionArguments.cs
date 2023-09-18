using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Dialogues;

using CombatDicesTeam.Dialogues;

namespace Client.GameScreens.Combat;

internal sealed record CombatScreenTransitionArguments(HeroCampaign Campaign,
    CombatSequence CombatSequence,
    int CurrentCombatIndex,
    bool IsAutoplay,
    GlobeNode Location,
    Dialogue<ParagraphConditionContext, AftermathContext>? VictoryDialogue) :
    CampaignScreenTransitionArgumentsBase(Campaign);