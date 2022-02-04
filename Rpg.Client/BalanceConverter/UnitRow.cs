using Rpg.Client.Core;

namespace BalanceConverter
{
    internal class UnitRow
    {
        public float DamageDealerRank { get; init; }
        public UnitName Sid { get; init; }

        public float SupportRank { get; init; }

        public float TankRank { get; init; }
    }
}