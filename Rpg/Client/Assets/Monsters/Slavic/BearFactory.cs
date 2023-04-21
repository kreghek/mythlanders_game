using Client.Assets;
using Client.Assets.Monsters;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class BearFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Bear;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.5f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.0f,

                Name = UnitName.Bear,
                LocationSids = new[]
                {
                    LocationSid.Battleground, LocationSid.DestroyedVillage, LocationSid.Swamp
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