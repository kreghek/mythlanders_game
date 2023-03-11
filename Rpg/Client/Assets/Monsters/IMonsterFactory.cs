using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    internal interface IMonsterFactory
    {
        public UnitName ClassName { get; }
        public UnitScheme Create(IBalanceTable balanceTable);
        UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage);
    }
}