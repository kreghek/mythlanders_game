using Client.GameScreens.Training;
using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;
using System.Linq;

namespace Client.Assets.StageItems
{
    internal sealed class TrainingStageItem : ICampaignStageItem
    {
        private readonly Player _player;
        private readonly IDice _dice;

        public TrainingStageItem(Player player, IDice dice)
        {
            _player = player;
            _dice = dice;
        }

        public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
        {
            var availableUnits = _player.GetAll().ToArray();

            var selectedAvailable = _dice.RollFromList(availableUnits, 3).ToList();

            var args = new TrainingScreenTransitionArguments(selectedAvailable, currentCampaign);

            screenManager.ExecuteTransition(currentScreen, ScreenTransition.Training, args);
        }
    }
}
