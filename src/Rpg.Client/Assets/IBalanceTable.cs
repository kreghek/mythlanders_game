using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal interface IBalanceTable
    {
        CommonUnitBasics GetCommonUnitBasics();
        BalanceTableRecord GetRecord(UnitName unitName);
    }
}