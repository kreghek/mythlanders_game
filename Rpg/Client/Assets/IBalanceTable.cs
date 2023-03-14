using Core.Balance;

namespace Client.Assets;

internal interface IBalanceTable
{
    CommonUnitBasics GetCommonUnitBasics();
    BalanceTableRecord GetRecord(string unitName);
}