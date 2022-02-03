using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal interface IHeroBuilder
    {
        UnitName UnitName { get; }
        UnitScheme Create(IBalanceTable balanceTable);
    }
}