using Client.Assets;
using Client.Core;
using Client.GameScreens;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Core;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class BasiliskFactory : MonsterFactoryBase
    {
        public override UnitName ClassName => UnitName.Basilisk;

        public override UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 1.0f,
                DamageDealerRank = 0.0f,
                SupportRank = 0.0f,

                Name = UnitName.Basilisk,
                LocationSids = new[]
                {
                    LocationSids.Pit, LocationSids.Swamp
                },
                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
            };
        }

        public override UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new SingleSpriteGraphicsConfig();
        }
    }
}