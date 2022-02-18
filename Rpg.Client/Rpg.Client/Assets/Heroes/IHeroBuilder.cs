using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal interface IHeroBuilder
    {
        UnitName HeroName { get; }
        UnitScheme Create(IBalanceTable balanceTable);
    }
}