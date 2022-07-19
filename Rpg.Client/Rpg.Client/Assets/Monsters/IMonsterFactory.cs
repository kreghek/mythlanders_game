using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    internal interface IMonsterFactory
    {
        public UnitScheme Create(IBalanceTable balanceTable);
    }
}
