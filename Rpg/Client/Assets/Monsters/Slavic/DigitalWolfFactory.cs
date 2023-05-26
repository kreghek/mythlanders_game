using Client.Assets;
using Client.Assets.GraphicConfigs.Monsters;
using Client.Assets.Monsters;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class DigitalWolfFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.DigitalWolf;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.DigitalWolf,
                LocationSids = new[]
                {
                    LocationSids.Thicket, LocationSids.Battleground, LocationSids.DestroyedVillage,
                    LocationSids.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                },

                UnitGraphicsConfig = new DigitalWolfGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new DigitalWolfGraphicsConfig();
        }
    }
}