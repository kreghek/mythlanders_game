using System.Collections.Generic;

using Client.Core.Campaigns;

using Rpg.Client.Core;
using Rpg.Client.Core.Dialogues;
using Rpg.Client.ScreenManagement;

using static Client.Core.Combat;

namespace Client.GameScreens.Combat;

internal sealed record CombatScreenTransitionArguments(HeroCampaign Campaign,
    CombatSequence CombatSequence,
    int CurrentCombatIndex,
    bool IsAutoplay,
    GlobeNode Location,
    IReadOnlyCollection<HeroHp> StartHpItems,
    Dialogue? VictoryDialogue) :
    CampaignScreenTransitionArgumentsBase(Campaign);