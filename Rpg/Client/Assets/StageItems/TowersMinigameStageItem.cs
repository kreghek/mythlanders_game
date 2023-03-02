﻿using Client.Core.Campaigns;
using Client.GameScreens.SlidingPuzzles;
using Client.GameScreens.TowersMinigame;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.StageItems;

internal sealed class TowersMinigameStageItem : ICampaignStageItem
{
    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.TowersMinigame,
            new TowersMiniGameScreenTransitionArguments(currentCampaign));
    }
}