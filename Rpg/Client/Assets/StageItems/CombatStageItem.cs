using System;

using Client.GameScreens.Combat;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

using static Client.Core.Combat;

namespace Client.Assets.StageItems;

internal sealed class CombatStageItem : ICampaignStageItem
{
    private readonly CombatSequence _combatSequence;
    private readonly GlobeNode _location;

    public CombatStageItem(GlobeNode location, CombatSequence combatSequence)
    {
        _location = location;
        _combatSequence = combatSequence;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat, new CombatScreenTransitionArguments(currentCampaign, _combatSequence, 0, false, _location, Array.Empty<HeroHp>(), null));
    }
}