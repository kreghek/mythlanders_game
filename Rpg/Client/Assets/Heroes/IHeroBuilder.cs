using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Client.Assets.Heroes;

internal interface IHeroFactory
{
    UnitName HeroName { get; }
    bool IsReleaseReady { get; }
    UnitScheme Create(IBalanceTable balanceTable);
    UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage);
}