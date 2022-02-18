using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal sealed class BalanceTableRecord
    {
        public float DamageDealerRank { get; init; }
        public UnitName Sid { get; init; }
        public float SupportRank { get; init; }
        public float TankRank { get; init; }
    }

    internal sealed class BalanceData
    {
        public BalanceTableRecord[] UnitRecords { get; init; }
        public CommonUnitBasics UnitBasics { get; init; }
    }
}