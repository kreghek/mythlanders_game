using Client.Core;
using Client.GameScreens;

using Rpg.Client.Core;

namespace Client.Assets.Heroes;

internal interface IHeroFactory
{
    UnitName HeroName { get; }
    bool IsReleaseReady { get; }
    UnitScheme Create(IBalanceTable balanceTable);
    UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage);
}