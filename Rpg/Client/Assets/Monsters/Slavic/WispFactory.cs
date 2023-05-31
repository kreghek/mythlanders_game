using Client.Assets;
using Client.Assets.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class WispFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Wisp;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Wisp,
                LocationSids = new[]
                {
                    LocationSids.DestroyedVillage, LocationSids.Swamp
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