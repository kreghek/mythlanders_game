using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal record BalanceData(CommonUnitBasics UnitBasics, BalanceTableRecord[] UnitRows);
}