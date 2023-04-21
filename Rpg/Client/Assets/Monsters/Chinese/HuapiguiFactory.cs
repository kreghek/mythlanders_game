using Client.Assets;
using Client.Assets.Monsters;
using Client.Core;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class HuapiguiFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Huapigui;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Huapigui,
                LocationSids = new[]
                {
                    LocationSids.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                },

                UnitGraphicsConfig = new GenericMonsterGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new GenericMonsterGraphicsConfig();
        }
    }
}