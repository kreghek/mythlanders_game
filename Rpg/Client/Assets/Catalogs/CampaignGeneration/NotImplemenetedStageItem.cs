﻿using Client.GameScreens.NotImplementedStage;

using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

namespace Client.Assets.Catalogs.CampaignGeneration;

/// <summary>
/// Temporal stage item to test campaign generation.
/// </summary>
internal sealed class NotImplemenetedStageItem : ICampaignStageItem
{
    public NotImplemenetedStageItem(string stageSid)
    {
        StageSid = stageSid;
    }

    /// <summary>
    /// Sid to display stage item's fake.
    /// </summary>
    public string StageSid { get; }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        screenManager.ExecuteTransition(currentScreen, ScreenTransition.NotImplemented,
            new NotImplementedStageScreenTransitionArguments(currentCampaign));
    }
}