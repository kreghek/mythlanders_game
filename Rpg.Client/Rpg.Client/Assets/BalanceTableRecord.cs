using Rpg.Client.Core;

namespace Rpg.Client.Assets
{
    internal sealed class BalanceTableRecord
    {
        public float DamageDealerRank { get; init; }
        public bool Demo { get; init; }
        public UnitName Sid { get; init; }
        public float SupportRank { get; init; }
        public float TankRank { get; init; }
        public bool Type { get; init; }
    }
}