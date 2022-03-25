namespace Rpg.Client.Assets
{
    internal sealed class BalanceData
    {
        public CommonUnitBasics UnitBasics { get; init; }
        public BalanceTableRecord[] UnitRows { get; init; }
    }
}