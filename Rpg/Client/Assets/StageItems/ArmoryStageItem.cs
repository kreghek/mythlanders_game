using System.Linq;

using Client.GameScreens.Armory;

using Core.Dices;

using Rpg.Client.Core;
using Rpg.Client.Core.Campaigns;
using Rpg.Client.ScreenManagement;

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