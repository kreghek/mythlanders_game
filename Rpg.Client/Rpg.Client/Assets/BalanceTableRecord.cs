
using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal sealed class BalanceTableRecord
    {
        public UnitName Sid { get; init; }
        public float SupportRank { get; init; }
        public float TankRank { get; init; }
        public float DamageDealerRank { get; init; }
    }
}
