using Client.Assets.Equipments.Archer;
using Client.Assets.Equipments.Sergeant;
using Client.Assets.Equipments.Swordsman;
using Client.Core;
using Client.Core.Campaigns;
using Client.GameScreens.Trade;
using Client.ScreenManagement;

using CombatDicesTeam.Dices;

namespace Client.Assets.StageItems;

internal sealed class TradingStageItem : ICampaignStageItem
{
    private readonly IDice _dice;
    private readonly Player _player;

    public TradingStageItem(Player player, IDice dice)
    {
        _player = player;
        _dice = dice;
    }

    public void ExecuteTransition(IScreen currentScreen, IScreenManager screenManager, HeroCampaign currentCampaign)
    {
        var equipments = new []
        {
            new Equipment(new CombatSword()),
            new Equipment(new ArcherPulsarBow()),
            new Equipment(new MultifunctionalClocks())
        };
        
        var args = new TradeScreenTransitionArguments(currentCampaign, equipments);

        screenManager.ExecuteTransition(currentScreen, ScreenTransition.Trade, args);
    }
}