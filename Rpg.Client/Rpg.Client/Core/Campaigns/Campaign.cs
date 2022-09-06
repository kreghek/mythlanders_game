using System.Collections.Generic;

using Rpg.Client.GameScreens.Combat;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.Core.Campaigns
{
    internal sealed class HeroCampaign
    {
        public IReadOnlyList<CampaignStage> CampaignStages { get; set; }
        public int CurrentStageIndex { get; set; }
    }

    internal sealed class CampaignStage
    {
        public IReadOnlyList<ICampaignStageItem> Items { get; set; }
        public string Title { get; set; }
    }

    internal interface ICampaignStageItem
    {
        void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager);
    }
}