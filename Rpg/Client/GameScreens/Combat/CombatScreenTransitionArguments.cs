using Client.Core.Campaigns;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;

namespace Client.GameScreens.Combat;

internal sealed record CombatScreenTransitionArguments(HeroCampaign Campaign,
    CombatSequence CombatSequence,
    int CurrentCombatIndex,
    bool IsAutoplay,
    GlobeNode Location,
    Dialogue? VictoryDialogue) :
    CampaignScreenTransitionArgumentsBase(Campaign);