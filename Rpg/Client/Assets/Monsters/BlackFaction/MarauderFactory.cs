using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class MarauderFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Marauder;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.5f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.0f,
                Resolve = 9,

                Name = UnitName.Marauder,
                LocationSids = new[]
                {
                    LocationSid.Thicket, LocationSid.Swamp, LocationSid.Battleground, LocationSid.DeathPath,
                    LocationSid.Mines,

                    LocationSid.Desert, LocationSid.SacredPlace,

                    LocationSid.ShipGraveyard,

                    LocationSid.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {

                },

                UnitGraphicsConfig = new MarauderGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new MarauderGraphicsConfig();
        }
    }
}