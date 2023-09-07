using Client.Core;
using Client.GameScreens;

namespace Client.Assets.Monsters;

internal interface IMonsterFactory
{
    public UnitName ClassName { get; }
    public UnitScheme Create(IBalanceTable balanceTable);
    UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage);
}