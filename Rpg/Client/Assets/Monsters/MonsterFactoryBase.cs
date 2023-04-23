using Client.Assets;
using Client.Assets.Monsters;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    internal abstract class MonsterFactoryBase : IMonsterFactory
    {
        public abstract UnitName ClassName { get; }

        public abstract UnitScheme Create(IBalanceTable balanceTable);

        public virtual UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new GenericMonsterGraphicsConfig();
        }
    }
}