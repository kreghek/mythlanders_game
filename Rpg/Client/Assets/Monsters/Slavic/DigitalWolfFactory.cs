using Client.Assets;
using Client.Assets.GraphicConfigs.Monsters;

using JetBrains.Annotations;

using Rpg.Client.Core;
using Rpg.Client.GameScreens;

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
                    LocationSid.Thicket, LocationSid.Battleground, LocationSid.DestroyedVillage,
                    LocationSid.Swamp
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