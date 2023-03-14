using Client.Assets;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Heroes
{
    internal interface IHeroFactory
    {
        UnitName HeroName { get; }
        bool IsReleaseReady { get; }
        UnitScheme Create(IBalanceTable balanceTable);
    }
}