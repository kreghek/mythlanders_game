using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Training;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

namespace Client.Assets.StageItems;

internal sealed class TrainingStageItem : ICampaignStageItem
{
    private readonly IDice _dice;
    private readonly Player _player;

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