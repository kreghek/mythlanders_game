using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class HydraFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.Hydra;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                Name = UnitName.Hydra,
                LocationSids = new[] { LocationSid.Labyrinth },

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