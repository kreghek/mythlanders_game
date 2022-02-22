using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal interface IHeroFactory
    {
        UnitName HeroName { get; }
        UnitScheme Create(IBalanceTable balanceTable);
    }
}