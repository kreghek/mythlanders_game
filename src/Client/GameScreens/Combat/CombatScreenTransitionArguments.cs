using Client.Core;
using Client.Core.Campaigns;
using Client.Core.Dialogues;

namespace Client.GameScreens.Combat;

internal sealed record CombatScreenTransitionArguments(HeroCampaign Campaign,
    CombatSequence CombatSequence,
    int CurrentCombatIndex,
    bool IsAutoplay,
    GlobeNode Location,
    Dialogue? VictoryDialogue) :
    CampaignScreenTransitionArgumentsBase(Campaign);