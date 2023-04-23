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
    internal sealed class VolkolakWarriorFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.VolkolakWarrior;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.5f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.0f,

                Name = UnitName.VolkolakWarrior,
                LocationSids = new[] { LocationSids.Swamp },
                IsUnique = true,
                IsMonster = true,
                MinRequiredBiomeLevel = 5,

                Levels = new IUnitLevelScheme[]
                {
                },

                UnitGraphicsConfig = new VolkolakWarriorGraphicsConfig(),

                SchemeAutoTransition = new UnitSchemeAutoTransition
                {
                    HpShare = 0.5f,
                    NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                    {
                        TankRank = 0.5f,
                        DamageDealerRank = 0.5f,
                        SupportRank = 0.0f,

                        Name = UnitName.Volkolak,
                        IsUnique = true,
                        IsMonster = true,

                        Levels = new IUnitLevelScheme[]
                        {
                        },

                        UnitGraphicsConfig = new VolkolakGraphicsConfig()
                    }
                }
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new VolkolakWarriorGraphicsConfig();
        }
    }
}