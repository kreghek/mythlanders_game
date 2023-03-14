using Client.Assets;

using JetBrains.Annotations;

using Rpg.Client.Assets.GraphicConfigs;
using Rpg.Client.Assets.GraphicConfigs.Monsters;
using Rpg.Client.Core;
using Rpg.Client.GameScreens;

namespace Rpg.Client.Assets.Monsters
{
    [UsedImplicitly]
    internal sealed class KosсheiTheDeathlessFactory : IMonsterFactory
    {
        public UnitName ClassName => UnitName.KosheyTheImmortal;

        public UnitScheme Create(IBalanceTable balanceTable)
        {
            return new UnitScheme(balanceTable.GetCommonUnitBasics())
            {
                TankRank = 0.3f,
                DamageDealerRank = 0.5f,
                SupportRank = 0.2f,

                Name = UnitName.KosheyTheImmortal,

                IsMonster = true,

                Levels = new IUnitLevelScheme[]
                {
                   
                },

                UnitGraphicsConfig = new SingleSpriteGraphicsConfig(),
                LocationSids = new[] { LocationSid.Castle },
                MinRequiredBiomeLevel = 10,

                SchemeAutoTransition = new UnitSchemeAutoTransition
                {
                    HpShare = 0.6f,
                    NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                    {
                        TankRank = 0.0f,
                        DamageDealerRank = 1.0f,
                        SupportRank = 0.0f,

                        Name = UnitName.KosheyTheImmortal2, // Dead-golem form
                        IsMonster = true,

                        Levels = new IUnitLevelScheme[]
                        {
                   
                        },

                        UnitGraphicsConfig = new KocheyDeadFormGraphicsConfig(),

                        SchemeAutoTransition = new UnitSchemeAutoTransition
                        {
                            HpShare = 0.3f,
                            NextScheme = new UnitScheme(balanceTable.GetCommonUnitBasics())
                            {
                                TankRank = 0.0f,
                                DamageDealerRank = 1.0f,
                                SupportRank = 0.0f,

                                Name = UnitName.KosheyTheImmortal3, // Gaint spiritual face
                                IsMonster = true,

                                Levels = new IUnitLevelScheme[]
                                {
                   
                                },

                                UnitGraphicsConfig = new SingleSpriteGraphicsConfig()
                            }
                        }
                    }
                }
            };
        }

        public UnitGraphicsConfigBase CreateGraphicsConfig(GameObjectContentStorage gameObjectContentStorage)
        {
            return new SingleSpriteGraphicsConfig();
        }
    }
}