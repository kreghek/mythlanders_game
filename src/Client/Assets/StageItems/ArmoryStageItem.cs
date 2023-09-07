using System.Linq;

using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Armory;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

namespace Client.Assets.StageItems;

internal sealed class ArmoryStageItem : ICampaignStageItem
{
    private readonly IDice _dice;
    private readonly GlobeProvider _globeProvider;

    public ArmoryStageItem(GlobeProvider globeProvider, IDice dice)
    {
        _globeProvider = globeProvider;
        _dice = dice;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var equipments = _globeProvider.Globe.Player.GetAll().SelectMany(x => x.Equipments).ToArray();

        var availableEquipments = _dice.RollFromList(equipments, 3).ToArray();

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Armory,
            new ArmoryScreenTransitionArguments(currentCampaign, availableEquipments));
    }
}