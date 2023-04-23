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
    internal sealed class BlackTrooperFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.BlackTrooper;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.25f,
                DamageDealerRank = 0.75f,
                SupportRank = 0.0f,
                Resolve = 9,

                Name = UnitName.BlackTrooper,
                LocationSids = new[]
                {
                    LocationSids.Thicket, LocationSids.Swamp, LocationSids.Battleground, LocationSids.DeathPath,
                    LocationSids.Mines,

                    LocationSids.Desert, LocationSids.SacredPlace,

                    LocationSids.ShipGraveyard,

                    LocationSids.Monastery
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                },

                UnitGraphicsConfig = new BlackTrooperGraphicsConfig()
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new BlackTrooperGraphicsConfig();
        }
    }
}