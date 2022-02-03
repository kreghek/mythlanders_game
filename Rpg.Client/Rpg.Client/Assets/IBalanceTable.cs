using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal interface IBalanceTable
    {
        BalanceTableRecord GetRecord(UnitName unitName);
    }
}