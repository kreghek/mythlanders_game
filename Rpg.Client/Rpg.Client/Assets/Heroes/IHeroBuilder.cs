using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal interface IHeroBuilder
    {
        UnitScheme Create(IBalanceTable balanceTable);
        UnitName UnitName { get; }
    }
}