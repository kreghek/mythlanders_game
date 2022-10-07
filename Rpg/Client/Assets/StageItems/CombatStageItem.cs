using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.GameScreens.CampaignSelection;
using Rpg.Client.GameScreens.Combat;
using Rpg.Client.ScreenManagement;

namespace Rpg.Client.Assets.StageItems
{
    internal sealed class CombatStageItem : ICampaignStageItem
    {
        private readonly GlobeNode _location;
        private readonly CombatSequence _combatSequence;
        private readonly ICampaignGenerator _campaignGenerator;

        public CombatStageItem(GlobeNode location, CombatSequence combatSequence, ICampaignGenerator campaignGenerator)
        {
            _location = location;
            _combatSequence = combatSequence;
            _campaignGenerator = campaignGenerator;
        }

        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
        {
            screenManager.ExecuteTransition(currentScreen, ScreenTransition.Combat, new CombatScreenTransitionArguments
            {
                CombatSequence = _combatSequence,
                Location = _location,
                CurrentCampaign = currentCampaign
            });
        }
    }
}