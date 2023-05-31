using Client.Assets;
using Client.Assets.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class TaoteSonFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Taote;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Taote,
                LocationSids = new[] { LocationSids.SkyTower },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new SingleSpriteGraphicsConfig();
        }
    }
}