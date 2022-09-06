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

    internal sealed class CombatStageItem : ICampaignStageItem
    {
        private readonly GlobeNode _location;
        private readonly CombatSequence _combatSequence;

        public CombatStageItem(GlobeNode location, CombatSequence combatSequence)
        {
            _location = location;
            _combatSequence = combatSequence;
        }

        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager)
        {
            screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat, new CombatScreenTransitionArguments
            {
                CombatSequence = _combatSequence,
                Location = _location
            });
        }
    }
}